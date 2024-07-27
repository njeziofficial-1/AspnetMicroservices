using Basket.Api.Entities;

namespace Basket.Api.Repositories;

public interface IBasketRepository
{
    Task<T?> GetBasket<T>(string userName) where T : class;
    Task<T?> UpdateBasket<T>(string? username, T basket) where T : class;
    Task DeleteBasket(string userName);
}
