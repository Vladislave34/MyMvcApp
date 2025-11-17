using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Data.Entities;

public class GoodsImageEntity : BaseEntity<int>
{
    public string Url { get; set; } = string.Empty;

    public int GoodsId { get; set; }
    
    public GoodsEntity Goods { get; set; } = null!;
}