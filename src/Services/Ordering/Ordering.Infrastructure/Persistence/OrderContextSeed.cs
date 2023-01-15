using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                await orderContext.Orders.AddRangeAsync(await GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private async static Task<IEnumerable<Order>> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order {UserName = "swn", FirstName = "Njezi", LastName = "Chigozie", EmailAddress = "n@n.com", AddressLine ="Boulevard", Country ="Nigeria", TotalPrice = 350m}
            }; 
        }
    }
}
