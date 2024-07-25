using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Repositories;

public class BasketRepository : IBasketRepository
{
    readonly IDistributedCache _redisCache;
    public Task DeleteBasket(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<ShoppingCart> GetBasket(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        throw new NotImplementedException();
    }
}
