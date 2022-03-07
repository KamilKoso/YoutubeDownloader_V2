using System.Threading;
using System.Threading.Tasks;

namespace YoutubeDownloader.Common.Dispatchers.QueryDispatcher
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
