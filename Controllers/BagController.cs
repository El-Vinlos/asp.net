using baitap.Services;
using Microsoft.AspNetCore.Mvc;

namespace baitap.Controllers;

public class BagController : Controller
{
    private readonly BagService _bagService;

    public BagController(BagService bagService)
    {
        _bagService = bagService;
    }

    public IActionResult Index()
    {
        var cart = _bagService.GetCart();
        return View(cart);
    }

    public IActionResult GetBagDetail()
    {
        var cart = _bagService.GetCart();
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
    public IActionResult AddToBag(int id)
    {
        var product = _bagService.GetProductById(id);
        if (product == null) return NotFound();

        _bagService.AddToBag(product, 1);
        return Ok();
    }
    
    [HttpPost]
    public IActionResult RemoveFromBag(int id)
    {
        _bagService.RemoveFromCart(id);
        return RedirectToAction(nameof(Index));
    }
}
