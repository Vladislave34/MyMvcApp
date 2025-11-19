using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyMvcApp.Data.Entities;

namespace MyMvcApp.Data.Entities;

[Table("tblOrderStatuses")]
public class OrderStatusEntity : BaseEntity<int>
{
    [StringLength(250)]
    public string Name { get; set; } = string.Empty;
}