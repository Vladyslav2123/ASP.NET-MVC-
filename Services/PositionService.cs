using HW_11.Interfaces;
using HW_11.Models;
using HW_11.Models.Form;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Services;

public class PositionService
{
	private readonly IPositionRepository _positionRepository;
	private readonly IUserRepository _userRepository;

	public PositionService(IPositionRepository context, IUserRepository userRepository)
	{
		_positionRepository = context;
		_userRepository = userRepository;
	}

	public async Task<Position> Get(int id)
	{
		var position = await _positionRepository.GetByIdAsync(id);

		return position;
	}

	public async Task<int> GetCountPosition(int id)
	{
		var list = await _userRepository.FindAsync(u => u.PositionId == id);

		var count = list.Count();
		return count;
	}

	public async Task<List<Position>> GetAll()
	{
		var list = await _positionRepository.GetAllAsync();
		return await list.ToListAsync();
	}

	private async Task<bool> ExistsAsync(string name, int? id = null)
	{
		var listSkill = await GetAll();
		if (listSkill != null)
		{
			var exists = listSkill.Exists(x => x.Title == name && x.Id != id);

			if (exists)
			{
				return true;
			}
		}
		return false;
	}

	public async Task<bool> Create(PositionForm form)
	{
		if (await ExistsAsync(form.Title!))
		{
			return false;
		}

		var position = new Position();
		position.Title = form.Title;

		await _positionRepository.AddAsync(position);
		await _positionRepository.SaveAsync();

		return true;
	}

	public async Task<bool> UpdatePosition(int id, PositionForm form)
	{
		if (await ExistsAsync(form.Title!, id))
		{
			return false;
		}

		var positon = await _positionRepository.GetByIdAsync(id);
		if (positon != null)
		{
			positon.Title = form.Title;
			await _positionRepository.UpdateAsync(positon);
		}
		await _positionRepository.SaveAsync();
		return true;
	}

	public async Task DeletePosition(int id)
	{
		var position = await _positionRepository.GetByIdAsync(id);
		if (position != null)
		{
			await _positionRepository.RemoveAsync(position);
		}
		await _positionRepository.SaveAsync();
	}
}