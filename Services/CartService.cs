using baitap.Models;
using baitap.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace baitap.Services;

public class CartService
{
    private readonly ISession _session;
    private const string CartKey = "CART";

    public CartService(IHttpContextAccessor accessor)
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

    public void AddToCart(Product product, int quantity)
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
                Price = product.Price,
                Quantity = quantity,
                Discount = 0, // you can calculate later
                Tax = 0       // same
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
}
