namespace Basket.Api.Entities;

public class ShoppingCart
{
    public ShoppingCart()
    {
        
    }
    public ShoppingCart(string? username)
    {
        Username = username;
    }
    public string? Username { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();

    public double TotalPrice
    {
        get
        {
            double totalPrice = 0;
            if (Items.Any())
                foreach (ShoppingCartItem item in Items)
                    totalPrice += item.Price * item.Quantity;
            return totalPrice;
        }
    }
}

public class ShoppingCartItem
{
    public int Quantity { get; set; }
    public string? Color { get; set; }
    public double Price { get; set; }
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }

}