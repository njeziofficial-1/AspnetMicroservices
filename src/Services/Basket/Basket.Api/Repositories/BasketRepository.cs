using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Repositories;

public class BasketRepository : IBasketRepository
{
    readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task DeleteBasket(string userName)
    => await _redisCache.RemoveAsync(userName);

    public async Task<T?> GetBasket<T>(string userName) where T : class
    {
        var basketString = await _redisCache.GetStringAsync(userName);
        if (string.IsNullOrEmpty(basketString))
            return null;
        return JsonSerializer.Deserialize<T>(basketString);
    }

    public async Task<T?> UpdateBasket<T>(string username, T basket) where T : class
    {
        await _redisCache.SetStringAsync(username, JsonSerializer.Serialize(basket));
        return await GetBasket<T>(username);
    }
}
