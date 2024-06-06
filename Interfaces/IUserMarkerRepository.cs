using HW_11.Models;

namespace HW_11.Interfaces;

public interface IUserMarkerRepository : IRepository<UserMarker>
{
	Task<List<UserMarker>> GetAll();
}
