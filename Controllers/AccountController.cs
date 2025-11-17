using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MyMvcApp.Constants;
using MyMvcApp.Data.Entities.Identity;
using MyMvcApp.Interfaces;
using MyMvcApp.Models.Account;

namespace MyMvcApp.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;

public class AccountController(UserManager<UserEntity> userManager,
    IImageService imageService,
    SignInManager<UserEntity> signInManager,
    IEmailSender _emailSender,
    IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult ForgotPassword() => View();
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("", "Введіть email.");
            return View();
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            
            return View("ForgotPasswordConfirmation");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var resetLink = Url.Action(
            "ResetPassword", 
            "Account", 
            new { token, email = user.Email }, 
            protocol: HttpContext.Request.Scheme);

        
        await _emailSender.SendEmailAsync(email, "Скидання пароля",
            $"Натисніть для відновлення пароля: <a href='{resetLink}'>Відновити пароль</a>");

        return View("ForgotPasswordConfirmation");
    }
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (token == null || email == null)
            return RedirectToAction("Index", "Main");

        return View(new ResetPasswordViewModel { Token = token, Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return View("ResetPasswordConfirmation");

        var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

        if (result.Succeeded)
            return View("ResetPasswordConfirmation");

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    public IActionResult ResetPasswordConfirmation() => View();
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Main");
            }
            
        }
        ModelState.AddModelError("", "дані не вірні");
        return View(model);
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = mapper.Map<UserEntity>(model);

        var imageStr = model.Image is not null 
            ? await imageService.UploadImageAsync(model.Image) : null;

        user.Image = imageStr;
        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            result = await userManager.AddToRoleAsync(user, Roles.User);
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Main");
        }
        else
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }
            return View(model);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Redirect("/");
    }
    
}