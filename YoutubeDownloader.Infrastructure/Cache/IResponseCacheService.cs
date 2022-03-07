using System;

namespace YoutubeDownloader.Infrastructure.Cache
{
    public interface IResponseCacheService
    {
        void CacheResponse(string cacheKey, object response, TimeSpan timeToLive);
        object GetCachedResponse(string cacheKey);
    }
}
