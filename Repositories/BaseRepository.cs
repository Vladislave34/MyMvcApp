using MyMvcApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data.Entities;

namespace MyMvcApp.Repositories;

public abstract class BaseRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool isDeleted = false)
    {
        return await _dbSet.Where(x=> x.IsDeleted == isDeleted).ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
        
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity == null)
            throw new Exception("Entity not found");

        
        var prop = entity.GetType().GetProperty("IsDeleted");
        if (prop != null)
        {
            prop.SetValue(entity, true);
            _dbSet.Update(entity);
        }
        else
        {
            
            _dbSet.Remove(entity);
        }

        await _dbContext.SaveChangesAsync();
    }

    public Task<IQueryable<TEntity>> GetAllQurableAsync()
    {
        throw new NotImplementedException();
    }
}