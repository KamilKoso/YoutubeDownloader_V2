using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using YoutubeDownloader.Api;
using YoutubeDownloader.Infrastructure.Database.Contexts;

namespace YoutubeDownloader.Integration.Tests.Utility
{
    [Collection("Sequential")]
    public abstract class TestHostFixture : IClassFixture<CustomWebApplicationFactory<TestStartup, Startup>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<TestStartup, Startup> _factory;
        protected readonly HttpClient Client;

        protected TestHostFixture()
        {
            _factory = new CustomWebApplicationFactory<TestStartup, Startup>();
            Client = _factory.CreateClient();
        }

        protected void GetContext(Action<YoutubeDownloaderContext> test)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<YoutubeDownloaderContext>();
                test(context);
            }
        }

        void IDisposable.Dispose()
        {
            GetContext(context =>
            {
                context.Database.EnsureDeleted();
            });
        }
    }
}
