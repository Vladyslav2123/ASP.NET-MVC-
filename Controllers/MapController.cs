using HW_11.Models;
using HW_11.Models.Rest;
using HW_11.Services;
using Microsoft.AspNetCore.Mvc;

namespace HW_11.Controllers;

[ApiController]
public class MapController : Controller
{
	private readonly MarkerService _markerService;

	public MapController(MarkerService markerService)
	{
		_markerService = markerService;
	}

	[HttpGet]
	[Route("/api/map/markers")]
	public async Task<ICollection<MapMarker>> List()
	{
		return await _markerService.GetMarkers();
	}

	[HttpPost]
	[Route("/api/map/marker")]
	public async Task<object> AddMarker([FromBody] MapMarker marker)
	{
		if (!ModelState.IsValid)
		{
			return new { Ok = false };
		}

		var markerNew = await _markerService.CreateAsync(marker);

		return new
		{
			Ok = true,
			Marker = markerNew
		};
	}

	[HttpDelete]
	[Route("/api/map/marker")]
	public async Task<object> DeleteMarker([FromBody] UserDeleteModel model)
	{
		await _markerService.DeleteAsync(model.id);

		return new
		{
			Ok = true
		};
	}
}