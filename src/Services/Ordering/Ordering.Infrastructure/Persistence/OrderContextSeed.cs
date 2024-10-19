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
                UserName = "njezi",
                FirstName = "Njezi",
                LastName ="Chigozie",
                TotalPrice = 350,
                EmailAddress = "njezichigozie@yahoo.com",
                AddressLine = "1 Unity Road, Elebu, Oyo State",
                Country = "Nigeria",
                State = "Oyo State",
                ZipCode = "1001001",
                CardName = "Njezi Chigozie",
                CardNumber = "090898767",
                Expiration = "04-09-2095",
                CVV = "123",
                PaymentMethod = 1
            },
            new()
            {
                UserName = "jaylo",
                FirstName = "Lovelyn",
                LastName ="Ugomma",
                TotalPrice = 789,
                EmailAddress = "ugommalovelyn@yahoo.com",
                AddressLine = "1 Unity Road, Elebu, Oyo State",
                Country = "Canada",
                State = "Ottawa",
                ZipCode = "7871223",
                CardName = "Lovelyn Ugomma",
                CardNumber = "1212343453",
                Expiration = "07-10-2295",
                CVV = "321",
                PaymentMethod = 2
            }
        };
}
