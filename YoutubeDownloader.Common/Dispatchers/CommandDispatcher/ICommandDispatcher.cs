using System.Threading;
using System.Threading.Tasks;

namespace YoutubeDownloader.Common.Dispatchers.CommandDispatcher
{
    public interface ICommandDispatcher
    {
        Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
    }
}
