using MyMvcApp.Areas.Admin.Models.User;
using MyMvcApp.Data.Entities.Identity;


namespace MyMvcApp.Interfaces;

public interface IUserService
{
    Task<List<UserItemModel>> GetUsersAsync();
    
    Task<UserItemModel> GetUserByIdAsync(int id);

    Task<bool> UpdateUserAsync(UserEditModel model);

}