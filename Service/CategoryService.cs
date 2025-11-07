using MyMvcApp.Interfaces;
using MyMvcApp.Models.Model;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data.Entities;
using MyMvcApp.Repositories;
namespace MyMvcApp.Service;
using AutoMapper;

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IImageService imageService): ICategoryService
{
    public async Task<List<CategoryItemModel>> GetAllAsync()
    {
        var ListTest = await categoryRepository.GetAllAsync();
        var model = mapper.Map<List<CategoryItemModel>>(ListTest);
        return model;
    }

    public async Task CreateAsync(CategoryCreateModel model)
    {
        var entity = await categoryRepository.FinByNameAsync(model.Name);

        if (entity != null)
        {
            throw new Exception("Category already exists");
        }
        
        entity = new CategoryEntity()
        {
            Name = model.Name
        };
        if (model.Image != null)
        {
            entity.Image = await imageService.UploadImageAsync(model.Image);
        }
        await categoryRepository.AddAsync(entity);
    }
    
    public async Task UpdateAsync(CategoryEditModel model)
    {
        var entity = await categoryRepository.FinByIdAsync(model.Id);

        if (entity == null)
            throw new Exception("Category not found");

        entity.Name = model.Name;

        if (model.Image != null)
        {
            
            if (!string.IsNullOrEmpty(entity.Image))
                await imageService.DeleteImageAsync(entity.Image);

            
            entity.Image = await imageService.UploadImageAsync(model.Image);
        }

        await categoryRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await categoryRepository.DeleteAsync(id);
    }
}