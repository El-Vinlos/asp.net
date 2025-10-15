using baitap.Models;
using baitap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace baitap.Controllers;

public class Products(FlorenciaDbContext context, BagService bagService, SearchService searchService) : Controller
{
    public IActionResult Index()
    {
        return View("Index");
    }
    
    public async Task<IActionResult> Search(string? query)
    {
        var results = await searchService.Search(query);
        return View(results);
    }
    
    [HttpGet]
    public async Task<JsonResult> Suggestions(string? term)
    {
        var suggestions = await searchService.Suggest(term);
        return Json(suggestions);
    }
    public async Task<IActionResult> Details(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost]
    public IActionResult AddToBag(int id, int quantity = 1)
    {
        var product = context.Products.Find(id);
        if (product == null) return NotFound();

        bagService.AddToBag(product, quantity);

        return RedirectToAction("Index", "Bag");
    }
}