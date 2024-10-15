using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    readonly IBasketRepository _repository;
    readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
    }

    [HttpGet("{username}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasket(string username)
    {
        var basket = await _repository.Get<ShoppingCart>(username);
        return Ok(basket ?? new ShoppingCart(username));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
    {
        /*TODO: Communicate with Discount.Grpc
         * and calculate latest price of product into shopping cart
        */

        basket.Items.ForEach(item =>
        {
            var coupon =  _discountGrpcService.GetDiscount(item.ProductName).Result;
            item.Price -= coupon.Amount;
        });

        return Ok(await _repository.Update(basket.Username, basket));
    }


    [HttpDelete("{username}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string username)
    {
        await _repository.Delete(username);
        return Ok($"Basket with username: {username} deleted successfully.");
    }
}
