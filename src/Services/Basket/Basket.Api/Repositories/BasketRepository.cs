
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Repositories;

public class BasketRepository : IBasketRepository
{
    readonly IDistributedCache _cache;

    public BasketRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task Delete(string username)
    => await _cache.RemoveAsync(username);

    public async Task<T?> Get<T>(string username) where T : class
    {
        var response = await _cache.GetStringAsync(username);
        if (string.IsNullOrEmpty(response))
            return default;
        return JsonSerializer.Deserialize<T?>(response);
    }

    public async Task<T?> Update<T>(string username, T value) where T : class
    {
        await _cache.SetStringAsync(username, JsonSerializer.Serialize(value));
        return await Get<T>(username);
    }
}
