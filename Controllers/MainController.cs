using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Data.Entities;
using MyMvcApp.Interfaces;
using MyMvcApp.Models.Model;

namespace MyMvcApp.Controllers;

public class MainController(MyAppContext db, IConfiguration configuration, IMapper mapper, IImageService imageService) : Controller
{
    // GET
    public async  Task<IActionResult> Index()
    {
        var list = await db.Categories
            .Where(c => !c.IsDeleted)
            .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
            
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateModel category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }
        
        string name = category.Name.Trim().ToLower();
        
        var entity = db.Categories.SingleOrDefault(c => c.Name.ToLower() == name);
        if (entity != null)
        {
            ModelState.AddModelError("", $"Category with name {name} already exists");
            return View(category);
        }
        
        entity = new CategoryEntity
        {
            Name = category.Name
        };
        var dir = configuration.GetValue<string>("DirImagePath");
        if (category.Image != null)
        {
            //var fileName = Guid.NewGuid().ToString()+".jpg";
            
            //var path = Path.Combine(Directory.GetCurrentDirectory(), dir ?? "image", fileName);
            
            //using var fileStream = new FileStream(path, FileMode.Create);
            //category.Image.CopyTo(fileStream);
            //categoryEntity.Image = fileName;
            
            entity.Image = await imageService.UploadImageAsync(category.Image);


        }

        
        db.Categories.Add(entity);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var category = db.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
            return NotFound();

        var dir = configuration.GetValue<string>("DirImagePath");
        // Видаляємо файл зображення якщо він існує
        if (!string.IsNullOrEmpty(category.Image))
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), dir ?? "image", category.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        category.IsDeleted = true;
        db.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var category = db.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
            return NotFound();

        var model = new CategoryEditModel
        {
            Id = category.Id,
            Name = category.Name,
            OldImage = category.Image
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var category = db.Categories.FirstOrDefault(c => c.Id == model.Id);
        if (category == null)
        {
            return NotFound();
        }
        string name = model.Name.Trim().ToLower();
        bool exists = db.Categories.Any(c => c.Id != model.Id && c.Name.ToLower() == name);
        if (exists)
        {
            ModelState.AddModelError("", $"Category '{model.Name}' already exists.");
            return View(model);
        }
        category.Name = model.Name;

        
        if (model.Image != null)
        {
            
            if (!string.IsNullOrEmpty(category.Image))
            {
                var dir = configuration.GetValue<string>("DirImagePath");
                string oldPath = Path.Combine(Directory.GetCurrentDirectory(), dir ?? "image", category.Image);

                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            
            category.Image = await imageService.UploadImageAsync(model.Image);
        }

        db.Categories.Update(category);
        await db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}