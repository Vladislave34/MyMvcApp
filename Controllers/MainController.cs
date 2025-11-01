using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Data;

namespace MyMvcApp.Controllers;

public class MainController(MyAppContext db) : Controller
{
    // GET
    public IActionResult Index()
    {
        var list = db.Categories.ToList();
        return View(list);
    }
}