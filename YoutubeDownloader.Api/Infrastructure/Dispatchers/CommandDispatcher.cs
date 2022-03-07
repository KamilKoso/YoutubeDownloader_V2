using System.Threading;
using System.Threading.Tasks;
using Autofac;
using YoutubeDownloader.Common.Dispatchers.CommandDispatcher;

namespace YoutubeDownloader.Api.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ILifetimeScope _lifetimeScope;

        public CommandDispatcher(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public async Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var handler = scope.Resolve<ICommandHandler<TCommand>>();
                await handler.Handle(command, cancellationToken);
            }
        }
    }
}