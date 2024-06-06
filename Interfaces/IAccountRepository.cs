using HW_11.Models;

namespace HW_11.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
	Task<IQueryable<Account>> GetAllAccount();
}