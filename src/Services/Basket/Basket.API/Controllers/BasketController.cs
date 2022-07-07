using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> Get(string userName)
        {
            return await _repository.Get(userName);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> Post(ShoppingCart basket)
        {
            // TODO: Communicate with Discount.gRPC
            // and calculate latest prices of product into shopping cart.
            // consume Discount gRPC

            // var discountTasks = basket.Items.Select(i => DoDiscount(i)).ToArray();

            // Task.WaitAll(discountTasks);

            foreach (var item in basket.Items)
            {
                await DoDiscount(item);
            }

            return await _repository.Update(basket);
        }

        private async Task DoDiscount(ShoppingCartItem item)
        {
            var coupon = await _discountGrpcService.Get(item.ProductName);
            item.Price -= coupon.Amount;
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> Delete(string userName)
        {
            await _repository.Delete(userName);

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
        {
            var basket = await _repository.Get(basketCheckout.UserName);

            if (basket == null)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            await _repository.Delete(basket.UserName);

            return Accepted();
        }
    }
}