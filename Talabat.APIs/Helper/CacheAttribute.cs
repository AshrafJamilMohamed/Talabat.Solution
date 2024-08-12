using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Service.Contract;

namespace Talabat.APIs.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLive;

        public CacheAttribute(int TimeToLive)
        {
            timeToLive = TimeToLive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Ask CLR For Creating Object From "ResponseCacheService" - Explicitly 
            var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            // Generate Key
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var Response = await responseCacheService.GetCachedResponse(cacheKey);

            // In Case of the response is already cached before
            if (!string.IsNullOrEmpty(Response))
            {
                var Contentresult = new ContentResult()
                {
                    Content = Response,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = Contentresult;
                return; // exit 
            }

            // In Case of the response is not  cached before
            // execute the EndPoint then cache the response
            var actionExecutedContext = await next.Invoke();

            // The return of the EndPoint
            if (actionExecutedContext.Result is OkObjectResult result && result.Value is { })
            {
                await responseCacheService.CacheResponseAsync(cacheKey, result.Value, TimeSpan.FromSeconds(timeToLive));
            }


        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var Key = new StringBuilder();
            Key.Append(request.Path);

            foreach (var (key, value) in request.Query)
            {
                Key.Append($"|{key}-{value}");

            }
            return Key.ToString();
        }
    }
}
