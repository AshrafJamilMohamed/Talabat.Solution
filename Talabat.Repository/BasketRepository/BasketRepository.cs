using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository.BasketRepository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
     
        public BasketRepository(IConnectionMultiplexer redis  )
        {

             _database = redis.GetDatabase();
            
        }

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var Basket = await _database.StringGetAsync(id);
            if (Basket.IsNullOrEmpty) return null;

            try
            {
                // Deserialize the basket into a CustomerBasket object
                var customerBasket = JsonSerializer.Deserialize<CustomerBasket>(Basket);
                
                return customerBasket;
            }
            catch (JsonException ex)
            {
 
                return null;
            }

            
        }

        // Create Or Update Customer Basket
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var CreatedOrUpdated = await _database.StringSetAsync(customerBasket.Id, JsonSerializer.Serialize(customerBasket), TimeSpan.FromDays(30));
            if (!CreatedOrUpdated) return null;

            return await GetBasketAsync(customerBasket.Id);
        }


        public async Task<bool> DeleteBasketAsync(string id)
            => await _database.KeyDeleteAsync(id);


    }
}
