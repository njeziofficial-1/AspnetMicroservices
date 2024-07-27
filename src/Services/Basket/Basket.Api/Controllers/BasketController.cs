using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    readonly IBasketRepository _repository;

    public BasketController(IBasketRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{username}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult>GetBasket(string username)
    {
        var basket = await _repository.GetBasket<ShoppingCart>(username);
        return Ok(basket ?? new ShoppingCart(username));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
    => Ok(await _repository.UpdateBasket(basket.UserName, basket));

    [HttpDelete("{username}", Name = "DeleteBasket")]
    public async Task<IActionResult>DeleteBasket(string username)
    {
        await _repository.DeleteBasket(username);
        return Ok($"Basket with username: {username}, deleted successfully.");
    }
}
