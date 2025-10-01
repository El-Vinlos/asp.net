using baitap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace baitap.Controllers;
public class SearchController : Controller
{
    private readonly FlorenciaDbContext _context;

    public SearchController(FlorenciaDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string query)
    {
        query = query?.Trim();  // normalize

        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new List<Product>()); // still works
        }

        var lowerQuery = query.ToLower();

        var results = _context.Products
            .Where(p => p.ProductName.ToLower().Contains(lowerQuery)
                     || p.Description.ToLower().Contains(lowerQuery)) // include Description
            .ToList();


        Console.WriteLine($"Found {results.Count} products for query '{query}'");
        return View(results);
    }
        [HttpGet]
        public JsonResult Suggestions(string term)
        {
            term = term?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<string>());

            var suggestions = _context.Products
                .Where(p => p.ProductName.ToLower().Contains(term)
                            || p.Description.ToLower().Contains(term))
                .Select(p => p.ProductName)
                .Take(5)
                .ToList();

            if (!suggestions.Any())
                Console.WriteLine("No suggestions found for term: {Term}", term);

            return Json(suggestions);
        }
}
