using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
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
            return await _repository.Update(basket);
        }
        
        [HttpDelete("{userName}")]
        public async Task<IActionResult> Delete(string userName)
        {
            await _repository.Delete(userName);

            return Ok();
        }
    }
}