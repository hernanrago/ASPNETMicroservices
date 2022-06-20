using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(
            IDiscountRepository repository,
            ILogger<DiscountController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{productName}")]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> GetAsync(string productName)
        {
            var item = await _repository.Get(productName);

            return item;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status201Created)]
        public async Task<ActionResult<Coupon>> CreateAsync(Coupon coupon)
        {
            await _repository.Create(coupon);

            return CreatedAtAction(nameof(CreateAsync), coupon);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Coupon coupon)
        {
            await _repository.Update(coupon);

            return Ok();
        }
        
        [HttpDelete("{productName}")]
        public async Task<IActionResult> DeleteAsync(string productName)
        {
            await _repository.Delete(productName);

            return Ok();
        }
    }
}