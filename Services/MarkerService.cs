using HW_11.Interfaces;
using HW_11.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Services;

public class MarkerService
{

	private readonly IMapMarkerRepository _mapMarkerRepository;
	private readonly IUserMarkerRepository _userMarkerRepository;

	public MarkerService(IUserMarkerRepository userMarkerRepository, IMapMarkerRepository mapMarkerRepository)
	{
		_userMarkerRepository = userMarkerRepository;
		_mapMarkerRepository = mapMarkerRepository;
	}

	public async Task<MapMarker> CreateAsync(MapMarker marker)
	{
		return await _mapMarkerRepository.AddAsync(marker);
	}

	public async Task DeleteAsync(int id)
	{
		var marker = await _mapMarkerRepository.GetByIdAsync(id);
		await _mapMarkerRepository.RemoveAsync(marker);
		await _mapMarkerRepository.SaveAsync();
	}

	public async Task<ICollection<UserMarker>> GetAll()
	{
		return await _userMarkerRepository.GetAll();
	}

	public async Task<ICollection<MapMarker>> GetMarkers()
	{
		var list = await _mapMarkerRepository.GetAllAsync();

		return await list.ToListAsync();
	}

	public async Task CreateUserMarker(UserMarker userMarker)
	{
		await _userMarkerRepository.AddAsync(userMarker);
		await _userMarkerRepository.SaveAsync();
	}
}