using AutoMapper;
using MyMvcApp.Data.Entities;
using MyMvcApp.Models.Model;

namespace MyMvcApp.Mappers;

public class CategoryMapper: Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryEntity, CategoryItemModel>();
    }
}