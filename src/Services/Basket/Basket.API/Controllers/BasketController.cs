using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IDiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository repository, IDiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBaskt(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //TODO: Communicate with  Discount.Grpc
            //Calculate latest prices of product into shopping cart.
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [HttpPost("[action]", Name = "CreateBasketBatch")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateBasketBatchAsync([FromBody] List<ShoppingCart> baskets)
        => Ok(await _repository.CreateBasketBatchAsync(baskets));

        /// <summary>
        /// Create a Basket
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        /// <remarks>
        /// {
 /// "userName": "swn",
  ///"items": [
    ///{
      ///"quantity": 2,
      ///"color": "Red",
      ///"price": 500,
      ///"productId": "60210c2a1556459e153f0554",
      ///"productName": "iPhone"
    ///},
	///{
      ///"quantity": 1,
      ///"color": "Blue",
      ///"price": 800,
      ///"productId": "60210c2a1556459e153f0555",
      ///"productName": "Samsung 10"
    ///}
  ///]
///}
        /// </remarks>
        [HttpPost("[action]", Name = "CreateBasket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateBasketAsync([FromBody] ShoppingCart basket)
        => Ok(await _repository.CreateBasketAsync(basket));

        /// <summary>
        /// Checkout basket
        /// </summary>
        /// <param name="basketCheckout"></param>
        /// <returns></returns>
        /// <remarks>
        /// {
  ///"userName": "swn",
  ///"totalPrice": 0,
  ///"firstName": "swn",
  ///"lastName": "swn",
  ///"emailAddress": "string",
  ///"addressLine": "string",
  ///"country": "string",
  ///"state": "string",
  ///"zipCode": "string",
  ///"cardName": "string",
  ///"cardNumber": "string",
  ///"expiration": "string",
  ///"cvv": "string",
  ///"paymentMethod": 1
///}
    /// </remarks>
    [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //Get existing basket with total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest(basket);
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            //Create basketCheckoutEvent -- Set TotalPrice on BasketCheckout eventMessage
            eventMessage.TotalPrice = basket.TotalPrice;
            // send checkout event to rabbitMq
            await _publishEndpoint.Publish(eventMessage);
            // remove the basket
            await _repository.DeleteBasket(basket.UserName);
            return Accepted();
        }
    }
}
