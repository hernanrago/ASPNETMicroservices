using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            this._context = context;
        }

        public Task Create(Product product)
        {
            return _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            var product = Get(id);

            var result = await _context.Products.DeleteOneAsync(p => p.Id == id);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<Product> Get(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var result = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}