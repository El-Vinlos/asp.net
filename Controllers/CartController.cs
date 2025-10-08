using baitap.Services;
using Microsoft.AspNetCore.Mvc;

namespace baitap.Controllers;

public class CartController : Controller
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        return View(cart);
    }

    public IActionResult GetCartSummary()
    {
        var cart = _cartService.GetCart();
        return Json(new {
            count = cart.Sum(c => c.Quantity),
            items = cart.Select(c => new {
                id = c.ProductId,
                name = c.ProductName,
                image = c.ImageUrl,
                price = c.Price,
                quantity = c.Quantity,
                total = c.Price * c.Quantity
            })
        });
    }
    
    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        var product = _cartService.GetProductById(id); // fetch from DB
        if (product == null) return NotFound();

        _cartService.AddToCart(product, 1);
        return Ok();
    }
    
    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        _cartService.RemoveFromCart(id);
        return RedirectToAction(nameof(Index));
    }
}
