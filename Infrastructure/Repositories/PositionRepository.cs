using HW_11.Infrastructure.DbContextApp;
using HW_11.Interfaces;
using HW_11.Models;

namespace HW_11.Infrastructure.Repositories;

public class PositionRepository : Repository<Position>, IPositionRepository
{
	public PositionRepository(ApplicationDBContext context) : base(context) { }
}
