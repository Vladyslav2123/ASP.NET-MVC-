using HW_11.Infrastructure.DbContextApp;
using HW_11.Infrastructure.Repositories;
using HW_11.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Interfaces;

public class UserMarkerRepository : Repository<UserMarker>, IUserMarkerRepository
{
	private readonly ApplicationDBContext _dbContext;

	public UserMarkerRepository(ApplicationDBContext context) : base(context)
	{
		_dbContext = context;
	}

	public async Task<List<UserMarker>> GetAll()
	{
		return await _dbContext.UserMarkers.Include(x => x.MapMarker).Include(x => x.Account).ToListAsync();
	}
}
