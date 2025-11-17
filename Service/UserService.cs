using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Areas.Admin.Models.User;
using MyMvcApp.Data;
using MyMvcApp.Data.Entities.Identity;
using MyMvcApp.Interfaces;


namespace MyMvcApp.Service;

using Microsoft.AspNetCore.Mvc;



public class UserService(MyAppContext context,
    IMapper mapper, IImageService imageService, UserManager<UserEntity> userManager,
RoleManager<RoleEntity> roleManager) : IUserService
{
    public async Task<List<UserItemModel>> GetUsersAsync()
    {
        //Це sql запит
        var query = context.Users;
        var model = await query
            .ProjectTo<UserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return model;
    }

    public async Task<UserItemModel> GetUserByIdAsync(int id)
    {
        var query = context.Users;
        var model = await query
            .ProjectTo<UserItemModel>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(item => item.Id == id)
            ;
        return model;
    }
    public async Task<UserEntity> GetUserEntityByIdAsync(int id)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateUserAsync(UserEditModel model)
    {
        var user = await userManager.Users
            .Include(u => u.UserRoles)
            .SingleOrDefaultAsync(u => u.Id == model.Id);

        if (user == null)
            return false;

        
        if (!string.IsNullOrEmpty(model.FullName))
        {
            var parts = model.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            user.FirstName = parts.ElementAtOrDefault(0);
            user.LastName = parts.ElementAtOrDefault(1);
        }

        
        user.Email = model.Email;
        user.UserName = model.Email; 
        user.NormalizedEmail = model.Email.ToUpper();
        user.NormalizedUserName = model.Email.ToUpper();

        
        if (model.ImageFile != null)
        {
            string newImage = await imageService.UpdateImageAsync(model.ImageFile, user.Image);
            user.Image = newImage;
        }

        
        if (model.Roles != null)
        {
            var oldRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, oldRoles);
            await userManager.AddToRolesAsync(user, model.Roles);
        }

        
        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}

