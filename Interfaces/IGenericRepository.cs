using MyMvcApp.Data.Entities;

namespace MyMvcApp.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : IEntity<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync(bool isDeleted = false);
    Task<IQueryable<TEntity>> GetAllQurableAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
}