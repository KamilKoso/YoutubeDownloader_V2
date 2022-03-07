using Microsoft.AspNetCore.Builder;
using YoutubeDownloader.Api.Middlewares;

namespace YoutubeDownloader.Api.Infrastructure
{
    public static class ApiBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
