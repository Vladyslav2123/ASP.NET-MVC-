using HW_11.Models;

namespace HW_11.Interfaces;

public interface IUserRepository : IRepository<UserInfo>
{
	Task<IQueryable<UserInfo>> GetAllWithInclude();
}
