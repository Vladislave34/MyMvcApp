using MyMvcApp.Data.Entities;

namespace MyMvcApp.Interfaces;

public interface ICategoryRepository : IGenericRepository<CategoryEntity, int>
{
    Task<CategoryEntity?> FinByNameAsync(string name);
    
    Task<CategoryEntity?> FinByIdAsync(int id);
}