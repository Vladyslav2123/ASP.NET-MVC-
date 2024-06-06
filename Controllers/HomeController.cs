using HW_11.Models;
using HW_11.Models.Form;
using HW_11.Models.Rest;
using HW_11.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HW_11.Controllers;

public class HomeController : Controller
{
	private readonly UserService _userService;

	public HomeController(UserService service)
	{
		_userService = service;
	}

	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	public async Task<IActionResult> Create()
	{
		ViewData["Positions"] = await _userService.GetAllPosition();
		return View(new UserForm());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([FromForm] UserForm form)
	{
		if (!ModelState.IsValid)
		{
			ViewData["Positions"] = await _userService.GetAllPosition();
			return View(form);
		}

		await _userService.Add(form);

		return RedirectToAction("ShowUsers");
	}

	[HttpGet]
	public async Task<IActionResult> Show(int id)
	{
		var user = _userService.GetAsync(id);
		ViewData["Skills"] = await _userService.GetUserSkill(id);
		ViewData["position"] = await _userService.GetPosition(user.PositionId);

		return View(user);
	}

	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		ViewData["Positions"] = await _userService.GetAllPosition();
		ViewData["Skills"] = await _userService.GetUserSkill(id);

		return View(_userService.GetAsync(id));
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, [FromForm] UserForm form)
	{
		if (!ModelState.IsValid)
		{
			ViewData["Positions"] = await _userService.GetAllPosition();
			return View(_userService.GetAsync(id));
		}

		await _userService.Update(id, form);

		return RedirectToAction("ShowUsers");
	}

	public async Task<IActionResult> ShowUsers()
	{
		ViewData["Positions"] = await _userService.GetAllPosition();

		var users = await _userService.GetAll();

		return View(users);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete([FromBody] UserDeleteModel user)
	{
		await _userService.Delete(user.id);

		return Json(new { ok = true });
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteImage([FromBody] ImageDeleteModel model)
	{
		await _userService.DeleteImage(model);

		return Json(new { ok = true });
	}

	[HttpPost]
	public async Task<IActionResult> Search([FromForm] string searchString)
	{
		if (string.IsNullOrEmpty(searchString))
		{
			ModelState.Remove("searchString");
			ModelState.AddModelError("searchString", "Введіть будь-який текст");
			return View("ShowUsers", new List<UserInfo>());
		}

		ViewData["Positions"] = await _userService.GetAllPosition();
		var users = await _userService.Search(searchString);

		if (users.Count <= 0)
		{
			ModelState.Remove("searchString");
			ModelState.AddModelError("searchString", "Не знайдено жодного користувача!!!");
		}

		return View("ShowUsers", users);
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}