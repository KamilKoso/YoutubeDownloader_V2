using System;
using System.Data;
using System.Reflection;
using Autofac;
using EnsureThat;
using YoutubeDownloader.Api.Controllers;
using YoutubeDownloader.Common.Auth;
using Microsoft.EntityFrameworkCore;
using Module = Autofac.Module;
using System.Net.Http;
using YoutubeDownloader.Application.Youtube.GetYoutubeVideoMetadata;
using YoutubeDownloader.Infrastructure.Database.Contexts;
using YoutubeDownloader.Infrastructure.Repositories;
using YoutubeDownloader.Infrastructure;
using YoutubeDownloader.Api.Infrastructure.Dispatchers;
using YoutubeDownloader.Infrastructure.Cache;
using YoutubeDownloader.Infrastructure.Queries;
using Microsoft.Data.SqlClient;
using System.Net;
using YoutubeDownloader.Infrastructure.QueryBuilder;

namespace YoutubeDownloader.Api.Infrastructure
{
    public class DefaultModule : Module
    {
        private readonly string _connectionString;

        public DefaultModule(string connectionString)
        {
            Ensure.String.IsNotNullOrWhiteSpace(connectionString, nameof(connectionString));

            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().AsImplementedInterfaces();

            RegisterContext(builder);
            RegisterServices(builder);
            RegisterControllers(builder);
            RegisterDispatchers(builder);
            RegisterRepositories(builder);
            RegisterHandlers(builder);
            RegisterHttpClient(builder);
            RegisterCache(builder);
            RegisterQueries(builder);
        }

        private static void RegisterTransientDependenciesAutomatically(
             ContainerBuilder builder,
             Assembly assembly,
             string nameSpace)
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith(nameSpace, StringComparison.InvariantCulture))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }

        private void RegisterContext(ContainerBuilder builder)
        {
            var options = new DbContextOptionsBuilder<YoutubeDownloaderContext>();
            options.UseSqlServer(_connectionString);

            builder.Register(container => new YoutubeDownloaderContext(options.Options, container.Resolve<ICurrentUserService>())).InstancePerLifetimeScope();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<CurrentUserService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        private void RegisterControllers(ContainerBuilder builder)
        {
            RegisterTransientDependenciesAutomatically(
                builder,
                typeof(YoutubeDownloaderController).Assembly,
                "YoutubeDownloader.Api.Controllers");
        }

        private void RegisterHttpClient(ContainerBuilder builder)
        {
            builder.Register<HttpClient>(x => new ()).AsSelf().SingleInstance();
        }

        private void RegisterCache(ContainerBuilder builder)
        {
            builder.RegisterType<ResponseCacheService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        private void RegisterQueries(ContainerBuilder builder)
        {
            builder.RegisterType<SqlQueryBuilder>().InstancePerDependency();
            builder.Register<IDbConnection>(x => new SqlConnection(_connectionString));

            RegisterTransientDependenciesAutomatically(
                builder,
                typeof(AudioDownloadHistoryQuery).Assembly,
                "YoutubeDownloader.Infrastructure.Queries");
        }

        private void RegisterDispatchers(ContainerBuilder builder)
        {
            RegisterTransientDependenciesAutomatically(
                builder,
                typeof(CommandDispatcher).Assembly,
                "YoutubeDownloader.Api.Infrastructure.Dispatchers");
        }


        private void RegisterHandlers(ContainerBuilder builder)
        {
            RegisterTransientDependenciesAutomatically(
               builder,
               typeof(YoutubeVideoMetadataHandler).Assembly,
               "YoutubeDownloader.Application");
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            RegisterTransientDependenciesAutomatically(
                builder,
                typeof(UserRepository).Assembly,
                "YoutubeDownloader.Infrastructure.Repositories");
        }
    }
}
