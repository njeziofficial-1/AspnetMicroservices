namespace Basket.Api.Entities;

public class ShoppingCart
{
    public ShoppingCart()
    {

    }
    public ShoppingCart(string? userName)
    {
        UserName = userName;
    }
    public string? UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = [];
    public double TotalPrice
    {
        get
        {
            double totalPrice = 0;
            foreach (var item in Items)
                totalPrice += item.Price * item.Quantity;
            return totalPrice;
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
}