using Learnify.Order.Domain.Entities;
namespace Learnify.Order.Application.Interfaces;

public interface IGenericRepository<TId, TEntity> where TId : struct where TEntity : BaseEntity<TId>
{
    Task<bool> AnyAsync(TId id);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize);
    ValueTask<TEntity?> GetByIdAsync(TId id);
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    ValueTask AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
