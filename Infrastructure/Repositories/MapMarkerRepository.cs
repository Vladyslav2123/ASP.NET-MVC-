using HW_11.Infrastructure.DbContextApp;
using HW_11.Infrastructure.Repositories;
using HW_11.Models;

namespace HW_11.Interfaces;

public class MapMarkerRepository : Repository<MapMarker>, IMapMarkerRepository
{
	public MapMarkerRepository(ApplicationDBContext context) : base(context)
	{
	}
}
