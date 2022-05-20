using System.Text.Json;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart> Get(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
            {
                return new ShoppingCart(userName);
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> Update(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

            return await Get(basket.UserName);
        }

        public Task Delete(string userName)
        {
            return _redisCache.RemoveAsync(userName);
        }
    }
}