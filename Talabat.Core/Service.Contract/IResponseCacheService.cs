namespace Talabat.Core.Service.Contract
{
    public interface IResponseCacheService
    {
        // Set The Caching
        Task CacheResponseAsync(string Key, object Response, TimeSpan TimeToLive);

        // Get The Caching
        Task<string?> GetCachedResponse(string Key);
    }
}
