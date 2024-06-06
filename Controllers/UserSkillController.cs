using HW_11.Models.Form;
using HW_11.Models.Rest;
using HW_11.Services;
using Microsoft.AspNetCore.Mvc;

namespace HW_11.Controllers;

public class UserSkillController : Controller
{
	private readonly UserSkillService _userSkillService;

	public UserSkillController(UserSkillService skillService)
	{
		_userSkillService = skillService;
	}

	[HttpGet]
	public async Task<IActionResult> CreateUserSkill(int id)
	{
		ViewData["UserId"] = id;

		var listSkill = await _userSkillService.WithoutUserSkill(id);

		if (listSkill.Count <= 0)
		{
			TempData["Error"] = "Не має більше навичок!!!";
			return RedirectToAction("Edit", new { controller = "Home", id });
		}

		ViewData["Skills"] = listSkill;

		return View(new UserSkillForm());
	}

	[HttpPost]
	public async Task<IActionResult> CreateUserSkill(int id, [FromForm] UserSkillForm form)
	{
		if (!ModelState.IsValid)
		{
			ViewData["UserId"] = id;
			ViewData["Skills"] = await _userSkillService.GetSkills();
			return View(form);
		}
		var exists = await _userSkillService.CreateUserSkill(form, id);

		if (!exists)
		{
			ViewData["UserId"] = id;
			ViewData["Skills"] = await _userSkillService.GetSkills();
			return View(form);
		}

		return RedirectToAction("Edit", new { controller = "Home", id });
	}

	[HttpGet]
	public IActionResult Edit(int id)
	{
		var skillUser = _userSkillService.Get(id);
		ViewBag.Name = skillUser.Skill.Name;

		return View(new UserSkillForm(skillUser));
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, [FromForm] UserSkillForm form)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.Id = id;
			return View(form);
		}

		await _userSkillService.Update(id, form);

		return RedirectToAction("Edit", new { controller = "Home", id });
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete([FromBody] UserDeleteModel model)
	{
		await _userSkillService.Delete(model.id);

		return Json(new { ok = true });
	}
}