using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
    var emailSender = scoped.ServiceProvider.GetRequiredService<IEmailSender>(); // <-- беремо сервіс

    db.Database.Migrate();
   
    await emailSender .SendEmailAsync("ostapchuk_vladyslav@gymnasia21.lutsk.ua", "Скидання пароля", "<b>Тест повідомлення</b>");

    if (!db.Roles.Any())
    {
        string[] roles = {"Admin", "User"};
        foreach (var item in roles)
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
    
}


app.Run();
