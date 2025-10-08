using baitap.Models;
using baitap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace baitap.Controllers;

public class ProductDetailController : Controller
{
    private readonly FlorenciaDbContext _context;
    private readonly BagService _bagService;

    public ProductDetailController(FlorenciaDbContext context, BagService bagService)
    {
        _context = context;
        _bagService = bagService;
    }

    // Product details page
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost]
    public IActionResult AddToBag(int id, int quantity = 1)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        _bagService.AddToBag(product, quantity);

        return RedirectToAction("Index", "Bag");
    }
}