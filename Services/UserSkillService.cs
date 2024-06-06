using HW_11.Interfaces;
using HW_11.Models;
using HW_11.Models.Form;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Services;

public class UserSkillService
{
	private readonly IUserSkillRepository _userSkillRepository;
	private readonly ISkillRepository _skillRepository;

	public UserSkillService(IUserSkillRepository userSkillRepository, ISkillRepository skillRepository)
	{
		_userSkillRepository = userSkillRepository;
		_skillRepository = skillRepository;
	}

	public async Task<bool> CreateUserSkill(UserSkillForm form, int id)
	{
		var userSkill = new UserSkill();
		userSkill.ProficiencyLevel = form.ProficiencyLevel;
		userSkill.SkillId = form.SkillId;
		userSkill.UserId = id;

		await _userSkillRepository.AddAsync(userSkill);
		await _userSkillRepository.SaveAsync();

		return true;
	}

	public async Task<List<Skill>> GetSkills()
	{
		var list = await _skillRepository.GetAllAsync();

		return await list.ToListAsync();
	}

	public async Task Delete(int id)
	{
		var userSkill = _userSkillRepository.FindInclude(x => x.Id == id, x => x.User, x => x.Skill);

		await _userSkillRepository.RemoveAsync(userSkill);
		await _skillRepository.SaveAsync();
	}

	public UserSkill Get(int id)
	{
		var skillUser = _userSkillRepository.FindInclude(x => x.Id == id, x => x.User, x => x.Skill);

		return skillUser;
	}

	public async Task Update(int id, UserSkillForm form)
	{
		var userSkill = await _userSkillRepository.GetByIdAsync(id);

		userSkill.ProficiencyLevel = form.ProficiencyLevel;

		await _userSkillRepository.UpdateAsync(userSkill);
		await _userSkillRepository.SaveAsync();
	}

	public async Task<List<Skill>> WithoutUserSkill(int id)
	{
		var list = await _userSkillRepository.FindSkill(id);

		return list;
	}
}