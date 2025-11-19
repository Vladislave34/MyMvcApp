using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data.Entities;
using MyMvcApp.Data.Entities.Identity;


namespace MyMvcApp.Data;

public class MyAppContext : IdentityDbContext<UserEntity, RoleEntity,
    int, IdentityUserClaim<int>, UserRoleEntity, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public MyAppContext(DbContextOptions<MyAppContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }
    public DbSet<CategoryEntity> Categories { get; set; }
    
    public DbSet<ProductEntity> Goods { get; set; }
    
    public DbSet<ProductImageEntity> GoodsImages { get; set; }
    
    public DbSet<CartEntity> Carts { get; set; }
    
    public DbSet<OrderStatusEntity> OrderStatuses { get; set; }
    
    public DbSet<OrderEntity> Orders { get; set; }
    
    public DbSet<OrderItemEntity> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.Role)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
        
        builder.Entity<CartEntity>()
            .HasKey(pi => new { pi.UserId, pi.ProductId });
        

    }

    
}