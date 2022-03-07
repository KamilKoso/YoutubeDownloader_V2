using System.Security.Claims;
using Autofac;
using IdentityServer4.AccessTokenValidation;
using YoutubeDownloader.Api.Infrastructure;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using YoutubeDownloader.Infrastructure.Hubs;

namespace YoutubeDownloader.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddHttpContextAccessor();
            services.AddSignalR();
            services.AddMemoryCache();

            ConfigureAuth(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var serilog = new LoggerConfiguration()
                  .MinimumLevel.Verbose()
                  .Enrich.FromLogContext()
                  .WriteTo.File(@"api_log.txt");

            loggerFactory.WithFilter(new FilterLoggerSettings
            {
                {"Microsoft", LogLevel.Warning},
                {"System", LogLevel.Warning}
            }).AddSerilog(serilog.CreateLogger());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseApiSecurityHttpHeaders();
            app.UseBlockingContentSecurityPolicyHttpHeader();
            app.RemoveServerHeader();
            app.UseNoCacheHttpHeaders();
            app.UseStrictTransportSecurityHttpHeader(env);
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(b => b.WithOrigins(Configuration["AppConfig:ClientUrl"]).AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseErrorHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MediaSendingHub>("/mediaProcessingProgress");
                endpoints.MapDefaultControllerRoute();
            });
        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["AppConfig:IdentityUrl"];
                    options.RequireHttpsMetadata = true;
                    options.ApiName = Instances.YoutubeDownloaderApi;
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ApiReader, policy => policy.RequireClaim("scope", Scopes.YoutubeDownloaderApi));
            });

            services.AddMvcCore(options =>
            {
                options.Filters.Add(new AuthorizeFilter(Policies.ApiReader));
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DefaultModule(Configuration.GetConnectionString("YoutubeDownloader")));
        }
    }
}
