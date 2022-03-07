using Microsoft.AspNetCore.Builder;

namespace YoutubeDownloader.Web.Infrastructure
{
    public static class ClientAppBuilderExtensions
    {
        public static IApplicationBuilder UseContentSecurityPolicyHttpHeader(this IApplicationBuilder application, AppConfig appConfig, AllowedContentSecurityPolicyHeader allowedContentSecurityPolicyHeader)
        {
            return application.UseCsp(options =>
            {
                options
                    .DefaultSources(x => x.None())
                    .ConnectSources(x =>
                    {
                        x.Self();
                        x.CustomSources(appConfig.IdentityUrl, appConfig.ApiUrl, appConfig.WebSocketUrl);
                    })
                    .FrameSources(x =>
                    {
                        x.Self();
                        x.CustomSources(appConfig.IdentityUrl);
                    })
                    .FontSources(x =>
                    {
                        x.Self();
                        x.CustomSources(allowedContentSecurityPolicyHeader.FontSources);

                    })
                    .ImageSources(x =>
                    {
                        x.Self();
                        x.CustomSources("data:");
                        x.CustomSources(allowedContentSecurityPolicyHeader.ImageSources);

                    })
                    .ScriptSources(x =>
                    {
                        x.Self();
                    })
                    .StyleSources(
                        x =>
                        {
                            x.Self();
                            x.CustomSources(allowedContentSecurityPolicyHeader.StyleSources);
                            x.UnsafeInlineSrc = true;
                        })
                    .FormActions(x =>
                    {
                        x.Self();
                    })
                    .FrameAncestors(x =>
                    {
                        x.Self();
                    })
                    .UpgradeInsecureRequests();
            });
        }
    }
}
