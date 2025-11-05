using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models.Model;

public class CategoryCreateModel
{
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Вкажіть назву категорії")]
    public string Name { get; set; } = string.Empty;
    //Передача на сервер фото
    [Display(Name = "Фото")]
    [Required(ErrorMessage ="Оберіть фото для категорії")]
    public IFormFile? Image { get; set; }

}