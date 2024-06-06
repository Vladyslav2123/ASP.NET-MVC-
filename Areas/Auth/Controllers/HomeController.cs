using HW_11.Areas.Auth.Models;
using HW_11.Areas.Auth.Models.Form;
using HW_11.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HW_11.Areas.Auth.Controllers;

[Area("Auth")]
public class HomeController : Controller
{
	private readonly UserManager<Account> _userManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;

	public HomeController(UserManager<Account> userManager, RoleManager<IdentityRole<int>> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	/// <summary>
	/// Login User.
	/// </summary>
	[HttpGet]
	public IActionResult Login(string? returnUrl)
	{
		return PartialView(new LoginForm());
	}

	[HttpPost]
	public async Task<IActionResult> Login([FromForm] LoginForm form, string? returnUrl)
	{
		if (!ModelState.IsValid)
		{
			return PartialView(form);
		}

		var user = await _userManager.FindByEmailAsync(form.Login);
		if (user == null)
		{
			ModelState.AddModelError(nameof(form.Login), "Користувач з таким логіном не існує");
			return PartialView(form);
		}

		if (!await _userManager.CheckPasswordAsync(user, form.Password))
		{
			ModelState.AddModelError(nameof(form.Login), "Пароль не дійсний");
			return PartialView(form);
		}

		await SignIn(user);

		if (returnUrl != null)
		{
			return Redirect(returnUrl);
		}

		this.AddToast("success", "Hello");
		return RedirectToAction("Index", "Home", new { area = "" });
	}

	/// <summary>
	/// Register new Account.
	/// </summary>
	[HttpGet]
	public IActionResult Register(string? returnUrl)
	{
		return PartialView(new RegisterForm());
	}

	[HttpPost]
	public async Task<IActionResult> Register([FromForm] RegisterForm form, string? returnUrl)
	{
		if (!ModelState.IsValid)
		{
			return PartialView(form);
		}

		if (form.Password != form.ConfirmPassword)
		{
			ModelState.AddModelError(nameof(form.ConfirmPassword), "Паролі повинні співпадати");
			return PartialView(form);
		}

		// Перевірка на те що такий користувач вже існує
		var user = await _userManager.FindByEmailAsync(form.Login);
		if (user != null)
		{
			ModelState.AddModelError(nameof(form.Login), "Користувач з таким логіном вже існує");
			return PartialView(form);
		}

		// Створення користувача
		user = new Account
		{
			Email = form.Login,
			UserName = form.Login,
			EmailConfirmed = true,
		};

		var result = await _userManager.CreateAsync(user, form.Password);

		if (!result.Succeeded)
		{
			ModelState.AddModelError(nameof(form.Login),
				string.Join(";", result.Errors.ToList()!.Select(x => x.Description))
				);
			return PartialView(form);
		}

		await SignIn(user);

		var count = _userManager.Users.Count();

		if (count > 0)
		{
			var roleUser = _roleManager.Roles.FirstOrDefault(x => x.Name == "User");
			await _userManager.AddToRoleAsync(user, roleUser!.Name!);
		}
		else
		{
			var roleUser = _roleManager.Roles.FirstOrDefault(x => x.Name == "Admin");
			await _userManager.AddToRoleAsync(user, roleUser!.Name!);
		}

		if (returnUrl != null)
		{
			return Redirect(returnUrl);
		}

		this.AddToast("success", "Hello");
		return RedirectToAction("Index", "Home", new { area = "" });
	}


	[HttpGet]
	[Authorize]
	public async Task<IActionResult> Logout(string? returnUrl)
	{
		await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

		return RedirectToAction("Index", "Home", new { area = "" });
	}

	private async Task SignIn(Account user)
	{
		var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
		identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
		identity.AddClaim(new Claim(ClaimTypes.Name, user!.UserName!));
		identity.AddClaim(new Claim(ClaimTypes.Email, user!.Email!));

		var userRoles = await _userManager.GetRolesAsync(user);
		foreach (var role in userRoles)
		{
			identity.AddClaim(new Claim(ClaimTypes.Role, role));
		}

		await HttpContext
			.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}