using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyMvcApp.Constants;
using MyMvcApp.Data;
using MyMvcApp.Data.Entities;
using MyMvcApp.Data.Entities.Identity;
using MyMvcApp.Interfaces;
using MyMvcApp.Repositories;
using MyMvcApp.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyAppContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<MyAppContext>()
    .AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapAreaControllerRoute(
    name: "AdminArea",
    areaName: "Admin",
    pattern: "admin/{controller=Dashboards}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();



string dirImagePath = builder.Configuration.GetValue<string>("DirImagePath") ?? "test";

Console.WriteLine(dirImagePath);

string path = Path.Combine(Directory.GetCurrentDirectory(), dirImagePath);
Directory.CreateDirectory(path);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = $"/{dirImagePath}"
});


using (var scoped = app.Services.CreateScope())
{
    var db = scoped.ServiceProvider.GetRequiredService<MyAppContext>();
    var RoleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    var signInManager = scoped.ServiceProvider.GetRequiredService<SignInManager<UserEntity>>();

    db.Database.Migrate();
   

    if (!db.Roles.Any())
    {
        
        foreach (var item in Roles.AllRoles)
        {
            var role = new RoleEntity(item);
            var result = await RoleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("Problems" + error.Description);
                }

                
            }
            else
            {
                Console.WriteLine($"Make role {item}");
            }
        }
        
    }

    if (!db.Users.Any())
    {
        var admin = new UserEntity
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            FirstName = "Super",
            LastName = "Admin",
            EmailConfirmed = true,
            Image = null
        };
        var result = await userManager.CreateAsync(admin, "Admin123!");
        if (result.Succeeded)
        {
            result = await userManager.AddToRoleAsync(admin, Roles.Admin);
            
            
        }
    }

    if (!db.Categories.Any())
    {
        var list = new List<CategoryEntity>()
        {
            new CategoryEntity()
            {
                Name = "Laptops",
                Image = "avqv3xqc.wrr.webp",
                
            },
            new CategoryEntity()
            {
                Name = "Phones",
                Image = "prxmmoww.pp0.webp",
            },
            new CategoryEntity()
            {
                Name = "Headphones",
                Image = "zo2mimuf.tk4.webp",
            }
        };
        await db.Categories.AddRangeAsync(list);
        await db.SaveChangesAsync();
    }

    if (!db.Goods.Any())
    {
        List<ProductEntity> list = new List<ProductEntity>()
        {
            new ProductEntity()
            {
                Name = "Macbook Pro 14 m4pro",
                Description = "Macbook Pro 14 is very powerful laptop",
                Price = 70999,
                CategoryId = db.Categories.First(c => c.Name == "Laptops").Id,
            },
            new ProductEntity()
            {
                Name = "Ipone 17",
                Description = "Ipone 17 is good option for you",
                Price = 40999,
                CategoryId = db.Categories.First(c => c.Name == "Phones").Id,
            },
            new ProductEntity()
            {
                Name = "Aippods pro 2",
                Description = "This headphone is made for for you",
                Price = 9999,
                CategoryId = db.Categories.First(c => c.Name == "Headphones").Id,
            }
            
        };
        await db.Goods.AddRangeAsync(list);
        await db.SaveChangesAsync();
    }
    if(!db.OrderStatuses.Any())
    {
        List<string> names = new List<string>() {
            "Нове", "Очікує оплати", "Оплачено",
            "В обробці", "Готується до відправки",
            "Відправлено", "У дорозі", "Доставлено",
            "Завершено", "Скасовано (вручну)", "Скасовано (автоматично)",
            "Повернення", "В обробці повернення" };

        var orderStatuses = names.Select(name => new OrderStatusEntity { Name = name }).ToList();

        await db.OrderStatuses.AddRangeAsync(orderStatuses);
        await db.SaveChangesAsync();
    }

    if (!db.Orders.Any())
    {
        var order = new OrderEntity()
        {
            OrderStatusId = db.OrderStatuses.First(o => o.Name == "Нове").Id,
            UserId = db.Users.First(u => u.UserName == "admin").Id,
        };
        await db.Orders.AddAsync(order);
        await db.SaveChangesAsync();
    }

    if (!db.OrderItems.Any())
    {
        var orderItems = new List<OrderItemEntity>()
        {
            new OrderItemEntity()
            {
                PriceBuy = 9999,

                OrderId = db.Orders.First().Id,
                ProductId = db.Goods.First(g => g.Name == "Aippods pro 2").Id,
                Count = 2
            },
            new OrderItemEntity()
            {
                PriceBuy = 40999,
                OrderId = db.Orders.First().Id,
                ProductId = db.Goods.First(g => g.Name == "Ipone 17").Id,
                Count = 1
            }
        };
        await db.OrderItems.AddRangeAsync(orderItems);
        await db.SaveChangesAsync();
    }
    
    
}


app.Run();
