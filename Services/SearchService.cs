using baitap.Models;
using Microsoft.EntityFrameworkCore;

namespace baitap.Services;

public class SearchService(FlorenciaDbContext context)
{
    public async Task<List<Product>> Search(string? query)
    {
        query = query?.Trim();

        if (string.IsNullOrWhiteSpace(query))
            return [];

        var lowerQuery = query.ToLower();

        var results = await context.Products
            .Where(p => p.Description != null &&
                        (p.ProductName.ToLower().Contains(lowerQuery) ||
                         p.Description.ToLower().Contains(lowerQuery)))
            .ToListAsync();
        
        return results;
    }
    
    public async Task<List<string>> Suggest(string? term)
    {
        term = term?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(term))
            return [];

        var suggestions = await context.Products
            .Where(p => p.Description != null && (p.ProductName.ToLower().Contains(term)
                                                  || p.Description.ToLower().Contains(term)))
            .Select(p => p.ProductName)
            .Take(5)
            .ToListAsync();
        
        if (suggestions.Count == 0)
            Console.WriteLine("No suggestions found for term: {term}");

        return suggestions;
    }

}