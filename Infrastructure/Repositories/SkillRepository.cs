using HW_11.Infrastructure.DbContextApp;
using HW_11.Interfaces;
using HW_11.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Infrastructure.Repositories;

public class SkillRepository : Repository<Skill>, ISkillRepository
{
	public SkillRepository(ApplicationDBContext context) : base(context) { }

	public async Task<IQueryable<Skill>> GetAllWithInclude()
	{
		return await Task.FromResult(_entities.AsQueryable().Include(x => x.Images));
	}
}
