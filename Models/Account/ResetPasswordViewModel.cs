namespace MyMvcApp.Models.Account;
using System.ComponentModel.DataAnnotations;
public class ResetPasswordViewModel
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має містити щонайменше 6 символів.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Паролі не співпадають.")]
    public string ConfirmPassword { get; set; } = null!;

    public string Token { get; set; } = null!;
}