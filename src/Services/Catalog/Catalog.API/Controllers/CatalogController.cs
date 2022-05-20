using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(
            IProductRepository repository,
            ILogger<CatalogController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
        {
            var items = await _repository.Get();

            return items.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> GetAsync(string id)
        {
            var item = await _repository.Get(id);

            if (item == null)
            {
                _logger.LogError("Product with id {0} not found.", id);
                return NotFound();
            }

            return item;
        }

        [HttpGet]
        [Route("category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategoryAsync(string category)
        {
            var items = await _repository.GetByCategory(category);

            return items.ToList();
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> CreateAsync(Product product)
        {
            await _repository.Create(product);

            return CreatedAtAction(nameof(CreateAsync), product);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            await _repository.Update(product);

            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _repository.Delete(id);

            return Ok();
        }
    }
}