using HW_11.Models;

namespace HW_11.Interfaces;

public interface IUserSkillRepository : IRepository<UserSkill>
{
	Task<List<Skill>> FindSkill(int id);
	Task<IQueryable<UserSkill>> GetUserSkills(int id);
}
