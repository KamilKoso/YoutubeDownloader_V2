using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using YoutubeDownloader.Common.Dispatchers.EventDispatcher;

namespace YoutubeDownloader.Api.Infrastructure.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger<EventDispatcher> _logger;

        public EventDispatcher(ILifetimeScope lifetimeScope, ILogger<EventDispatcher> logger)
        {
            _lifetimeScope = lifetimeScope;
            _logger = logger;
        }
        public async Task Dispatch<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                @event = @event ?? throw new ArgumentNullException(nameof(@event));
                var eventType = @event.GetType();
                var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType).MakeArrayType();
                var handlers = scope.Resolve(handlerType);
                @event.MarkAsDispatched();

                foreach (var handler in (IEnumerable)handlers)
                {
                    try
                    {
                        Task task = (Task)handler.GetType().InvokeMember(
                            "Handle",
                            BindingFlags.InvokeMethod,
                            Type.DefaultBinder,
                            handler,
                            new object[] { @event },
                            CultureInfo.CurrentCulture
                        );
                        await task;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{handler.GetType()} - Exception occured, {ex}");
                    }
                }
            }
        }
    }
}
