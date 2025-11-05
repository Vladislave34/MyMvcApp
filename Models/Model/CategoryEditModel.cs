namespace MyMvcApp.Models.Model;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class CategoryEditModel
{
    public int Id { get; set; } // Id категорії для редагування

    [Required(ErrorMessage = "Назва категорії обов'язкова")]
    [StringLength(100, ErrorMessage = "Назва не може бути довшою за 100 символів")]
    public string Name { get; set; } = null!; // Назва категорії

    [Display(Name = "Зображення")]
    public IFormFile? Image { get; set; } // Нове зображення (якщо користувач завантажує)

    public string? OldImage { get; set; } // Ім'я старого зображення для прев’ю або видалення
}