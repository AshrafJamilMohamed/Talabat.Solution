using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Service.Contract;

namespace Talabat.Service.CachingService
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase database;
        // Cache The Response 
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string Key, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return;
            var JsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var SerializedRsponse = JsonSerializer.Serialize(Response, JsonOptions);
            await database.StringSetAsync(Key, SerializedRsponse, TimeToLive);
        }

        public async Task<string?> GetCachedResponse(string Key)
        {

            var Response = await database.StringGetAsync(Key);
            if (Response.IsNullOrEmpty) return null;
            return Response;
        }
    }
}
