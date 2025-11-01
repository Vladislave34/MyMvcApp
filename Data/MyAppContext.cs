using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data.Entities;
namespace MyMvcApp.Data;

public class MyAppContext : DbContext
{
    public MyAppContext(DbContextOptions<MyAppContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }
    public DbSet<CategoryEntity> Categories { get; set; }
}