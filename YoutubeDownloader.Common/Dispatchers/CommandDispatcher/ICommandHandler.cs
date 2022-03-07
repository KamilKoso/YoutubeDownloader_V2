using System.Threading;
using System.Threading.Tasks;

namespace YoutubeDownloader.Common.Dispatchers.CommandDispatcher
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }
}
