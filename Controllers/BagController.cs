using baitap.Services;
using Microsoft.AspNetCore.Mvc;

namespace baitap.Controllers;

public class BagController(BagService bagService) : Controller
{
    public IActionResult Index()
    {
        var cart = bagService.GetBag();
        return View(cart);
    }

    public IActionResult GetBagDetail()
    {
        var cart = bagService.GetBag();
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
        if (!bagService.TryAddToBag(id))
        {
            return NotFound();
        }
        return Ok();
    }
    
    [HttpPost]
    public IActionResult RemoveFromBag(int id)
    {
        bagService.RemoveFromBag(id);
        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            return Ok(new { success = true });
        return RedirectToAction(nameof(Index));
    }
}
