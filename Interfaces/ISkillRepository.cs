using HW_11.Models;

namespace HW_11.Interfaces;

public interface ISkillRepository : IRepository<Skill>
{
	Task<IQueryable<Skill>> GetAllWithInclude();
}
