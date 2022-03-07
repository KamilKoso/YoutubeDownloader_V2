using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EnsureThat;
using YoutubeDownloader.Common.Dispatchers;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;

namespace YoutubeDownloader.Api.Infrastructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly ILifetimeScope _lifetimeScope;

        public QueryDispatcher(ILifetimeScope lifetimeScope)
        {
            EnsureArg.IsNotNull(lifetimeScope, nameof(lifetimeScope));

            _lifetimeScope = lifetimeScope;
        }

        public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var handlerExists = TryGetQueryHandler(_lifetimeScope, query, out object handler);

            if (!handlerExists)
            {
                throw new Exception($"Handler for query {GetQueryName(query)} does not exist.");
            }

            return await ExecuteHandler(handler, query, cancellationToken);
        }

        protected virtual async Task<TResult> ExecuteHandler<TResult>(object handler, IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = (Task<TResult>)handler.GetType()
                    .GetRuntimeMethod("Handle", new[] { query.GetType(), typeof(CancellationToken) })
                    .Invoke(handler, new object[] { query, cancellationToken });

                return await result;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        private static bool TryGetQueryHandler<TResult>(ILifetimeScope scope, IQuery<TResult> query, out object handler)
        {
            var asyncGenericType = typeof(IQueryHandler<,>);
            var closedAsyncGeneric = asyncGenericType.MakeGenericType(query.GetType(), typeof(TResult));

            return scope.TryResolve(closedAsyncGeneric, out handler);
        }

        private static string GetQueryName(object query)
        {
            return query.GetType().Name;
        }
    }
}