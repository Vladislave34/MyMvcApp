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
    
    public DbSet<GoodsEntity> Goods { get; set; }
    
    public DbSet<GoodsImageEntity> GoodsImages { get; set; }

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
        
        builder.Entity<GoodsEntity>()
            .HasMany(g => g.Images)
            .WithOne(i => i.Goods)
            .HasForeignKey(i => i.GoodsId)
            .OnDelete(DeleteBehavior.Cascade);

    }

    
}