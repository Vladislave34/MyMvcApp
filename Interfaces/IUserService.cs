using MyMvcApp.Data.Entities.Identity;
using MyMvcApp.Models.User;

namespace MyMvcApp.Interfaces;

public interface IUserService
{
    Task<List<UserItemModel>> GetUsersAsync();
    
    Task<UserItemModel> GetUserByIdAsync(int id);

    Task<bool> UpdateUserAsync(UserEditModel model);

}