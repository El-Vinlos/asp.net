using baitap.Models;
using baitap.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace baitap.Services;

public class BagService(IHttpContextAccessor accessor, FlorenciaDbContext context)
{
    private readonly ISession _session = accessor.HttpContext!.Session;
    private const string BagKey = "BAG";

    public List<BagItem> GetBag()
    {
        var data = _session.GetString(BagKey);
        if (string.IsNullOrEmpty(data)) return new List<BagItem>();

        return JsonSerializer.Deserialize<List<BagItem>>(data)!;
    }

    private void SaveBag(List<BagItem> cart)
    {
        _session.SetString(BagKey, JsonSerializer.Serialize(cart));
    }

    public void AddToBag(Product product, int quantity)
    {
        var cart = GetBag();

        var existing = cart.FirstOrDefault(c => c.ProductId == product.ProductId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cart.Add(new BagItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = quantity,
                Discount = 0,
                Tax = 0
            });
        }

        SaveBag(cart);
    }

    public bool TryAddToBag(int id)
    {
        var product = GetProductById(id);
        if (product == null) return false;
        
        AddToBag(product,1);
        return true;
    }

    public void RemoveFromBag(int id)
    {
        var cart = GetBag();
        var item = cart.FirstOrDefault(c => c.ProductId == id);
        if (item == null) return;
        cart.Remove(item);
        SaveBag(cart);
    }

    public Product GetProductById(int id)
    {
        return context.Products.FirstOrDefault(p => p.ProductId == id);
    }
}
