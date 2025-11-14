using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models.User;

public class UserEditModel
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    
    public IFormFile? ImageFile { get; set; }

    
    public string Image { get; set; } = string.Empty;

    public string[]? Roles { get; set; }

    public List<string> AllRoles { get; set; } = new();
}