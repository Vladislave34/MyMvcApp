namespace MyMvcApp.Areas.Admin.Models.User;

public class UserItemModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string[]? Roles { get; set; }
    
    public List<string>? AllRoles { get; set; }
}