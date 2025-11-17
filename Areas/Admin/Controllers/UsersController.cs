using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Constants;
using MyMvcApp.Interfaces;
using MyMvcApp.Areas.Admin.Models.User;

namespace MyMvcApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = $"{Roles.Admin}")]
public class UsersController(IUserService userService, IImageService imageService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var result = await userService.GetUsersAsync();
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await userService.GetUserByIdAsync(id);

        var model = new UserEditModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Image = user.Image,   // <- string
            Roles = user.Roles,
            AllRoles = Roles.AllRoles.ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserEditModel model)
    {
        if(!ModelState.IsValid)
            return View(model);

        bool updated = await userService.UpdateUserAsync(model);

        if (!updated)
        {
            ModelState.AddModelError("", "Помилка при оновленні користувача.");
            return View(model);
        }

        return RedirectToAction("Index");
    }
}