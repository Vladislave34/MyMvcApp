using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Data.Entities;
[Table("tblGoods")]
public class GoodsEntity : BaseEntity<int>
{
    [StringLength(255)]
    public string Name { get; set; } =  string.Empty;
    
    
    public List<GoodsImageEntity> Images { get; set; } = new();
    
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;
}