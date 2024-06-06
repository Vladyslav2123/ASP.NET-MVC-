using System.Linq.Expressions;

namespace HW_11.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
	Task<TEntity> GetByIdAsync(int id);
	Task<IQueryable<TEntity>> GetAllAsync();
	Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
	Task<TEntity> AddAsync(TEntity entity);
	Task AddRangeAsync(IEnumerable<TEntity> entities);
	Task UpdateAsync(TEntity entity);
	Task RemoveAsync(TEntity entity);
	Task RemoveRangeAsync(IEnumerable<TEntity> entities);
	Task SaveAsync();
	TEntity FindInclude(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties);
}