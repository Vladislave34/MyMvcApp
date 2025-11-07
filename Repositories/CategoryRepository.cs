using MyMvcApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Data.Entities;

namespace MyMvcApp.Repositories;

public class CategoryRepository : BaseRepository<CategoryEntity, int>, ICategoryRepository
{
    public CategoryRepository(MyAppContext dbContext)
        : base(dbContext)
    { }

    public async Task<CategoryEntity?> FinByNameAsync(string name)
    {
        var entity = await _dbSet.SingleOrDefaultAsync(c=> c.Name==name.ToLower().Trim());
        return entity;
    }

    public async Task<CategoryEntity?> FinByIdAsync(int id)
    {
        var  entity = await _dbSet.SingleOrDefaultAsync(c=> c.Id == id);
        return entity;
    }
}