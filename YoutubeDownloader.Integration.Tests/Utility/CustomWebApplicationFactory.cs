using System;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YoutubeDownloader.Infrastructure.Database.Contexts;
using YoutubeDownloader.Integration.Tests.Data;

namespace YoutubeDownloader.Integration.Tests.Utility
{
    public class CustomWebApplicationFactory<TTestStartup, TStartup> : WebApplicationFactory<TStartup>
        where TTestStartup : class
        where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder(null)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TTestStartup>();
                });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var appsettingsFileName = "appsettings.json";

            builder.ConfigureServices(services =>
            {
                services.AddMvc().AddApplicationPart(typeof(TStartup).Assembly);

                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkSqlServer()
                    .BuildServiceProvider();

                var connectionString = new ConfigurationBuilder()
                     .AddJsonFile(appsettingsFileName)
                     .Build()
                     .GetConnectionString("YoutubeDownloader");

                services.AddDbContext<YoutubeDownloaderContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var appDb = scopedServices.GetRequiredService<YoutubeDownloaderContext>();

                    appDb.Database.EnsureDeleted();
                    appDb.Database.Migrate();

                    try
                    {
                        // Seed the database with some specific test data.
                        DataHelper.PopulateTestData(appDb);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred seeding the database with test messages. Error: {ex.InnerException.Message}");
                    }
                }
            }).ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), appsettingsFileName));
            });
        }
    }
}
