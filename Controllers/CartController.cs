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
    [HttpGet("Cart/Remove/{id}")]
    public IActionResult Remove(int id)
    {
        _cartService.RemoveFromCart(id);
        return RedirectToAction(nameof(Index));
    }
}
