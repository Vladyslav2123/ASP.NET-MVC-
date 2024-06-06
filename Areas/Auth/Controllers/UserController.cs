using HW_11.Areas.Auth.Models;
using HW_11.Areas.Auth.Models.Form;
using HW_11.Models;
using HW_11.Models.Rest;
using HW_11.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace HW_11.Areas.Auth.Controllers;

[Area("Auth")]
[Authorize]
public class UserController : Controller
{
	private readonly UserManager<Account> _userManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;
	private readonly AccountService _accountService;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly MarkerService _markerService;

	public UserController(UserManager<Account> userManager, RoleManager<IdentityRole<int>> roleManager, AccountService accountService, IHttpClientFactory httpClientFactory, MarkerService markerService)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_accountService = accountService;
		_httpClientFactory = httpClientFactory;
		_markerService = markerService;
	}

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var list = await _accountService.GetAccounts();

		return View(list);
	}

	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var user = _accountService.GetAsync(id);

		var userForm = new UserEditForm(user!);

		var roles = await _roleManager.Roles.ToListAsync();
		var userRoles = await _userManager.GetRolesAsync(user!);
		userForm.UserRoles = roles.Select(
			x => new UserRolesForm
			{
				Name = x.Name!,
				IsActived = userRoles.Contains(x.Name!),
			}).ToList();

		ViewBag.Id = id;

		return PartialView("Edit", userForm);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, [FromForm] UserEditForm form)
	{
		if (!ModelState.IsValid)
		{
			return PartialView("Edit", form);
		}

		var user = _accountService.GetAsync(id);

		if (user == null)
		{
			return RedirectToAction("Index");
		}

		user.UserName = form.Name;
		user.Email = form.Email;

		if (form.Image != null)
		{
			//await _accountService.GetImageAsync(form.Image);

			if (user.Image != null)
			{
				await _accountService.DeleteImage(new ImageDeleteModel { Id = id, Src = user.Image.GetImage() });
			}

			user.Image = await _accountService.GetImageAsync(form.Image);
		}

		foreach (var item in form.UserRoles)
		{
			if (item.IsActived)
			{
				if (!await _userManager.IsInRoleAsync(user, item.Name))
				{
					await _userManager.AddToRoleAsync(user, item.Name);
				}
			}
			else
			{
				if (await _userManager.IsInRoleAsync(user, item.Name))
				{
					await _userManager.RemoveFromRoleAsync(user, item.Name);
				}
			}
		}

		await _userManager.UpdateAsync(user);

		return RedirectToAction("Index");
	}

	[HttpGet]
	public async Task<IActionResult> CreateAsync()
	{
		var roles = await _roleManager.Roles.ToListAsync();
		var form = new UserCreateForm
		{
			UserRoles = roles.Select(
				x => new UserRolesForm
				{
					Name = x.Name!,
					IsActived = false,
				}).ToList(),
		};
		return PartialView("Create", form);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([FromForm] UserCreateForm form)
	{
		if (!ModelState.IsValid)
		{
			this.AddToast("error", "Невірно заповнена форма");
			return PartialView("Create", form);
		}

		if (form.Password != form.ConfirmPassword)
		{
			this.AddToast("error", "Паролі повинні співпадати");
			ModelState.AddModelError(nameof(form.ConfirmPassword), "Паролі повинні співпадати");
			return PartialView("Create", form);
		}

		var user = await _userManager.FindByEmailAsync(form.Email);
		if (user != null)
		{
			ModelState.AddModelError(nameof(form.Email), "Користувач з таким логіном вже існує");
			return PartialView("Create", form);
		}

		var client = _httpClientFactory.CreateClient();

		var markerData = new MapMarker
		{
			Title = form.Name,
			Lat = form.MapMarker.Lat,
			Lng = form.MapMarker.Lng
		};

		var jsonMarkerData = JsonConvert.SerializeObject(markerData);

		var response = await client.PostAsync("https://localhost:7121/api/map/marker", new StringContent(jsonMarkerData, Encoding.UTF8, "application/json"));

		if (response.IsSuccessStatusCode)
		{
			var responseContent = await response.Content.ReadAsStringAsync();

			var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

			markerData.Id = responseObject!.marker.id;
			markerData.Title = responseObject!.marker.title;
			markerData.Lat = responseObject!.marker.lat;
			markerData.Lng = responseObject!.marker.lng;
		}

		user = new Account
		{
			Email = form.Email,
			UserName = form.Name,
			EmailConfirmed = true,
		};

		if (form.Image != null)
		{
			user.Image = await _accountService.GetImageAsync(form.Image);
		}

		var result = await _userManager.CreateAsync(user, form.Password);

		if (!result.Succeeded)
		{
			ModelState.AddModelError(nameof(form.Email),
				string.Join(";", result.Errors.ToList()!.Select(x => x.Description))
				);
			return PartialView("Create", form);
		}

		foreach (var role in form.UserRoles.Where(x => x.IsActived))
		{
			await _userManager.AddToRoleAsync(user, role.Name);
		}

		var userMarker = new UserMarker
		{
			MapMarkerId = markerData.Id,
			AccountId = user.Id,
		};

		await _markerService.CreateUserMarker(userMarker);

		return RedirectToAction("Index");
	}

	[HttpGet]
	public IActionResult ModalProfile(int id)
	{
		var account = _accountService.GetAsync(id);

		return PartialView("ModalProfile", account);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteImage([FromBody] ImageDeleteModel model)
	{
		await _accountService.DeleteImage(model);

		return Json(new { ok = true });
	}

	[HttpGet]
	public IActionResult Delete(int id)
	{
		return PartialView(_userManager.Users.ToList());
	}

	[HttpPost]
	public IActionResult Delete([FromForm] UserRolesForm form)
	{
		return RedirectToAction("Index");
	}
}