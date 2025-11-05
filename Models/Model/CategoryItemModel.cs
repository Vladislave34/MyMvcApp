namespace MyMvcApp.Models.Model;
using System.ComponentModel.DataAnnotations;

public class CategoryItemModel
{
    [Display(Name = "#")]
    public int Id { get; set; }
    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Фото")]
    public string Image { get; set; } = string.Empty;
}