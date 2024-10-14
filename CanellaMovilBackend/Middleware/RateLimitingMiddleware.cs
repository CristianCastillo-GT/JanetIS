using CanellaMovilBackend.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CanellaMovilBackend.Middleware
{
    /// <summary>
    /// Middleware para controlar la tasa de conexiones.
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// </summary>
        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        /// <summary>
        /// </summary>
        public async Task Invoke(HttpContext httpContext)
        {
            string userId = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";

            int requestLimit = 100;
            TimeSpan timeWindow = TimeSpan.FromMinutes(1);

            if (httpContext?.Request.Headers.ContainsKey("STODNET") ?? false)
            {
                if (httpContext != null)
                    await _next(httpContext);
                return;
            }

            if (_cache.TryGetValue(userId, out RateLimitEntry? entry))
            {
                if (entry?.RequestCount >= requestLimit)
                {
                    if (httpContext != null)
                        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    LogsAPI.Info("*-BLOCK IP-*");
                    LogsAPI.Info($"IP: "+ userId + ", Peticiones: " + entry.RequestCount);
                    LogsAPI.Info("*-Le BLOCKE IP-*");
                    return;
                }

                if (entry != null)
                    entry.RequestCount++;
                if (_cache != null)
                    _ = _cache.Set(userId, entry, timeWindow);
            }
            else
            {
                entry = new RateLimitEntry { RequestCount = 1 };
                if (_cache != null)
                    _ = _cache.Set(userId, entry, timeWindow);
            }
            if (httpContext != null)
                await _next(httpContext);
        }
    }

    /// <summary>
    /// </summary>
    public class RateLimitEntry
    {
        /// <summary>
        /// </summary>
        public int RequestCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class RateLimitingMiddlewareExtensions
    {
        /// <summary>
        /// </summary>
        public static IApplicationBuilder UseRateLimitingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}
