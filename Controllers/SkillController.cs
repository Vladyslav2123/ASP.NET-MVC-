using HW_11.Models.Form;
using HW_11.Models.Rest;
using HW_11.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HW_11.Controllers
{
	public class SkillController : Controller
	{
		private readonly SkillService _skillService;

		public SkillController(SkillService skillService)
		{
			_skillService = skillService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var list = await _skillService.GetAll();
			return View(list);
		}

		[HttpGet]
		[Authorize]
		public IActionResult Create()
		{
			return View(new SkillForm());
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create([FromForm] SkillForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}
			var exists = await _skillService.CreateAsync(form);

			if (!exists)
			{
				ModelState.AddModelError(nameof(form.Name), "Така навичка вже існує!");
				return View(form);
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize]
		public IActionResult Edit(int id)
		{
			ViewBag.Id = id;
			var skill = new SkillForm(_skillService.Get(id));
			return View(skill);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EditAsync(int id, [FromForm] SkillForm form)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Id = id;
				return View(form);
			}

			var exists = await _skillService.UpdateSkill(id, form);

			if (!exists)
			{
				ModelState.AddModelError(nameof(form.Name), "Така навичка вже існує!");
				ViewBag.Id = id;
				return View(form);
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete([FromBody] SkillDeleteModel model)
		{
			var userCount = await _skillService.GetCountSkill(model.id);

			if (userCount > 0)
			{
				return Json(new { ok = false, Error = "Can`t delete skill" });
			}

			await _skillService.DeleteSkill(model.id);

			return Json(new { ok = true });
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteImage([FromBody] ImageDeleteModel model)
		{
			await _skillService.DeleteImage(model);

			return Json(new { ok = true });
		}
	}
}