using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyMvcApp.Data.Entities;
[Table("tblCategory")]
public class CategoryEntity : BaseEntity<int>
{
    
    [StringLength(255)]
    public string Name { get; set; } =  string.Empty;
    
    [StringLength(255)]
    public string Image { get; set; } = string.Empty;
    
    
    
    
}