using baitap.Models;
using baitap.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace baitap.Services;

public class BagService
{
    private readonly ISession _session;
    private readonly FlorenciaDbContext _context;
    private const string CartKey = "CART";

    public BagService(IHttpContextAccessor accessor)
    {
        _session = accessor.HttpContext!.Session;
    }

    public List<CartItem> GetCart()
    {
        var data = _session.GetString(CartKey);
        if (string.IsNullOrEmpty(data)) return new List<CartItem>();

        return JsonSerializer.Deserialize<List<CartItem>>(data)!;
    }

    private void SaveCart(List<CartItem> cart)
    {
        _session.SetString(CartKey, JsonSerializer.Serialize(cart));
    }

    public void AddToBag(Product product, int quantity)
    {
        var cart = GetCart();

        var existing = cart.FirstOrDefault(c => c.ProductId == product.ProductId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
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

        SaveCart(cart);
    }

    public void RemoveFromCart(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == id);
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }
    }

    public Product GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.ProductId == id);
    }
}
