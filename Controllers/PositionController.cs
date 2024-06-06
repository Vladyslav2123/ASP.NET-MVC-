using HW_11.Models;
using HW_11.Models.Form;
using HW_11.Models.Rest;
using HW_11.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HW_7.Controllers
{
	[Authorize]
	public class PositionController : Controller
	{
		private readonly PositionService _positionService;

		public PositionController(PositionService service)
		{
			_positionService = service;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _positionService.GetAll());
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View(new PositionForm());
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] PositionForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var exists = await _positionService.Create(form);

			if (!exists)
			{
				ModelState.AddModelError(nameof(form.Title), "Така позиція вже існує!");
				return View(form);
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			return View(new PositionForm(await _positionService.Get(id)));
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, [FromForm] PositionForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var exists = await _positionService.UpdatePosition(id, form);

			if (!exists)
			{
				ModelState.AddModelError(nameof(form.Title), "Така позиція вже існує!");
				return View(form);
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete([FromBody] PositionDeleteModel position)
		{

			var userCount = await _positionService.GetCountPosition(position.id);

			if (userCount > 0)
			{
				return Json(new { ok = false, Error = "Can`t delete position" });
			}

			await _positionService.DeletePosition(position.id);

			return Json(new { ok = true });
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}