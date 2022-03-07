using IdentityModel;
using YoutubeDownloader.Common.Auth;
using Microsoft.AspNetCore.Http;

namespace YoutubeDownloader.Api.Infrastructure
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimTypes.Subject)?.Value;
        }

        public string UserId { get; }
    }
}
