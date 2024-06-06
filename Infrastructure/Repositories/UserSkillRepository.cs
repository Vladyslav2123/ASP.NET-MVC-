using HW_11.Infrastructure.DbContextApp;
using HW_11.Interfaces;
using HW_11.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Infrastructure.Repositories;

public class UserSkillRepository : Repository<UserSkill>, IUserSkillRepository
{
	private readonly ApplicationDBContext _context;

	public UserSkillRepository(ApplicationDBContext context) : base(context)
	{
		_context = context;
	}

	public async Task<List<Skill>> FindSkill(int id)
	{
		var list = await _context.Skills
						.Where(s => !_context.UserSkills
						.Any(us => us.SkillId == s.Id && us.UserId == id))
						.ToListAsync();
		return list;
	}

	public async Task<IQueryable<UserSkill>> GetUserSkills(int id)
	{
		return await Task.FromResult(_context.UserSkills.Where(x => x.UserId == id).Include(x => x.User).Include(x => x.Skill));
	}
}
