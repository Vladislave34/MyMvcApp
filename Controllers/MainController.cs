using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Data.Entities;
using MyMvcApp.Interfaces;
using MyMvcApp.Models.Model;

namespace MyMvcApp.Controllers;

public class MainController(
    MyAppContext db,
    IConfiguration configuration,
    IMapper mapper,
    IImageService imageService, 
    ICategoryService categoryService) : Controller
{
    // GET
    public async  Task<IActionResult> Index()
    {
        var model  = await categoryService.GetAllAsync();
        //var list = await db.Categories
         //   .Where(c => !c.IsDeleted)
          //  .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
           // .ToListAsync();
            
        return View(model);
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

        try
        {
            await categoryService.CreateAsync(category);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(category);
        }
        
        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await categoryService.DeleteAsync(id);

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

        try
        {
            await categoryService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
        return RedirectToAction("Index");
    }
}