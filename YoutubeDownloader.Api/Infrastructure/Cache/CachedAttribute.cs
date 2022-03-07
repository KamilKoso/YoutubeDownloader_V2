using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.Common.Constans;
using YoutubeDownloader.Infrastructure.Cache;

namespace YoutubeDownloader.Api.Infrastructure.Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
        private readonly CacheScope _scope;

        public CachedAttribute(int timeToLiveInSeconds, CacheScope scope = CacheScope.Global)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
            _scope = scope;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Only get request can be cached
            if (!HttpMethods.IsGet(context.HttpContext.Request.Method))
            {
                await next();
                return;
            }

            // If request is cached, return it.
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var currentUserService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request, currentUserService);
            var serverCacheControlHeader = context.HttpContext.Request.Headers[CustomHeaders.ServerCacheControl].ToString();
            var cacheResponse = serverCacheControlHeader.Contains(CacheControlOptions.CacheRefresh) ? null : cacheService.GetCachedResponse(cacheKey);

            if (cacheResponse is not null)
            {
                await context.HttpContext.Response.WriteAsJsonAsync(cacheResponse);
                return;
            }

            // If it is not, execute request
            var executedContext = await next();

            // Request was not cached - cache it.
            if (executedContext.Result is OkObjectResult okObjectResult && !serverCacheControlHeader.Contains(CacheControlOptions.DontCache))
            {
                cacheService.CacheResponse(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request, ICurrentUserService currentUserService)
        {
            var keyBuilder = new StringBuilder();

            if (_scope == CacheScope.User)
            {
                keyBuilder.Append($"userId-{currentUserService.UserId}|");
            }

            keyBuilder.Append(request.Path);

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
