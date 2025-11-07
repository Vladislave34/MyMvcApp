using MyMvcApp.Models.Model;

namespace MyMvcApp.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryItemModel>> GetAllAsync();
    Task CreateAsync(CategoryCreateModel model);
    
    Task UpdateAsync(CategoryEditModel model);
    
    Task DeleteAsync(int id);
}