using HW_11.Infrastructure.DbContextApp;
using HW_11.Interfaces;
using HW_11.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Infrastructure.Repositories;

public class AccountRepository : Repository<Account>, IAccountRepository
{
	private readonly ApplicationDBContext _dBContext;

	public AccountRepository(ApplicationDBContext context) : base(context)
	{
		_dBContext = context;
	}

	public async Task<IQueryable<Account>> GetAllAccount()
	{
		return await Task.FromResult(_dBContext.Users.Include(x => x.Image));
	}
}