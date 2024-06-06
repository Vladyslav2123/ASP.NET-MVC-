using HW_11.Infrastructure.DbContextApp;
using HW_11.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HW_11.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
	private readonly ApplicationDBContext _context;
	protected readonly DbSet<TEntity> _entities;

	public Repository(ApplicationDBContext context)
	{
		_context = context;
		_entities = context.Set<TEntity>();
	}

	public async Task<TEntity> GetByIdAsync(int id)
	{
		return await _entities.FindAsync(id);
	}

	public async Task<IQueryable<TEntity>> GetAllAsync()
	{
		return await Task.FromResult(_entities.AsQueryable());
	}

	public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
	{
		return await Task.FromResult(_entities.Where(predicate).AsQueryable());
	}

	public async Task<TEntity> AddAsync(TEntity entity)
	{
		var user = await _entities.AddAsync(entity);
		await _context.SaveChangesAsync();
		return user.Entity;
	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
	{
		await _entities.AddRangeAsync(entities);
	}

	public async Task UpdateAsync(TEntity entity)
	{
		_entities.Update(entity);
		await Task.CompletedTask;
	}

	public async Task RemoveAsync(TEntity entity)
	{
		_entities.Remove(entity);
		await Task.CompletedTask;
	}

	public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
	{
		_entities.RemoveRange(entities);
		await Task.CompletedTask;
	}

	public async Task SaveAsync()
	{
		await _context.SaveChangesAsync();
	}

	public TEntity FindInclude(
	Expression<Func<TEntity, bool>> expression,
	params Expression<Func<TEntity, object>>[] includeProperties)
	{
		IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();

		if (includeProperties != null)
		{
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
		}
		return query.Where(expression).FirstOrDefault()!;
	}
}
