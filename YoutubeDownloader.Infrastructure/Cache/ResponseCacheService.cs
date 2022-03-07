using Microsoft.Extensions.Caching.Memory;
using System;

namespace YoutubeDownloader.Infrastructure.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public ResponseCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void CacheResponse(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return;
            }


            _memoryCache.Set(cacheKey, response, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public object GetCachedResponse(string cacheKey)
        {
            return _memoryCache.Get(cacheKey);

        }
    }
}
