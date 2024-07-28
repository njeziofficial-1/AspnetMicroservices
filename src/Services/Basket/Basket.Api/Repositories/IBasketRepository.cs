namespace Basket.Api.Repositories;

public interface IBasketRepository
{
    Task<T> Get<T>(string username) where T : class;
    Task<T> Update<T>(string username, T value) where T : class;
    Task Delete(string username);
}
