using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
        }
    }

    static IEnumerable<Order> GetPreconfiguredOrders()
        => new List<Order>
        {
            new()
            {
                UserName = "Njezi",
                FirstName = "Lord",
                LastName ="Chigozie",
                EmailAddress = "njezichigozie@yahoo.com",
                AddressLine = "1 Unity Road, Elebu, Oyo State",
                Country = "Nigeria",
                TotalPrice = 350
            }
        };
}
