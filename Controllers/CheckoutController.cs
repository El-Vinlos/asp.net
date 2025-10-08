using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace baitap.Controllers;

public class CheckoutController : Controller
{
    public IActionResult Index()
    {
        return View("Checkout");
    }
}
