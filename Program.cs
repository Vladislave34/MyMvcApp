using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Data.Entities;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyAppContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();

string dirImgePath = builder.Configuration.GetValue<string>("DirImagePath") ?? "test";

Console.WriteLine(dirImgePath);

using (var scoped = app.Services.CreateScope())
{
    var db = scoped.ServiceProvider.GetRequiredService<MyAppContext>();
    db.Database.Migrate();

    if(!db.Categories.Any())
    {
        var categories = new List<CategoryEntity>
        {
            new CategoryEntity 
            { 
                Name = "Напої безалкогольні", 
                Image = "https://src.zakaz.atbmarket.com/cache/category/%D0%91%D0%B5%D0%B7%D0%B0%D0%BB%D0%BA%D0%BE%D0%B3%D0%BE%D0%BB%D1%8C%D0%BD%D1%96%20%D0%BD%D0%B0%D0%BF%D0%BE%D1%96%CC%88.webp"
            },
            new CategoryEntity
            {
                Name = "Овочі та фрукти",
                Image = "https://src.zakaz.atbmarket.com/cache/category/%D0%9E%D0%B2%D0%BE%D1%87%D1%96%20%D1%82%D0%B0%20%D1%84%D1%80%D1%83%D0%BA%D1%82%D0%B8.webp"
            }
        };
        db.Categories.AddRange(categories);
        db.SaveChanges();
    }
    
}


app.Run();
