using baitap.Models;
using baitap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace baitap.Controllers;

public class ProductController : Controller
{
    private readonly FlorenciaDbContext _context;
    private readonly CartService _cartService;

    public ProductController(FlorenciaDbContext context, CartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    // Product details page
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product == null) return NotFound();

        return View(product);
    }

    // Add to cart (POST)
    [HttpPost]
    public IActionResult AddToCart(int id, int quantity = 1)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        _cartService.AddToCart(product, quantity);

        // Redirect back to cart page (or product page)
        return RedirectToAction("Index", "Cart");
    }
}