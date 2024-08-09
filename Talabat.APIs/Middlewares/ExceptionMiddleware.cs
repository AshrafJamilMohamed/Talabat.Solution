using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly RequestDelegate _Next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment, RequestDelegate next)
        {
            _logger = logger;
            _environment = environment;
            _Next = next;
        }

        // Must have this Function
        public async Task InvokeAsync(HttpContext HttpContext)
        {
            try
            {
                await _Next.Invoke(HttpContext); // Go to the Next Middleware

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message); // Development Enviroment
                                              // Log Exception in [Database||files]   => Production Env

                HttpContext.Response.StatusCode = 500;
                HttpContext.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment() ?
                       new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
                        :
                       new ApiExceptionResponse(500);

                // To make Each Property in Json File as CamelCase
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await HttpContext.Response.WriteAsync(json);
            }
        }
    }
}
