namespace baitap.ViewModel;

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public string? ImageUrl { get; set; }

    public decimal Subtotal => Quantity * Price;
    public decimal Total => (Subtotal - Discount) + Tax;
}
