using System.Threading;
using System.Threading.Tasks;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;

namespace YoutubeDownloader.Common.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
