using YoutubeDownloader.Application.DownloadHistory.GetAudioDownloadHistory;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;
using YoutubeDownloader.Domain.Common.Pagination;

namespace YoutubeDownloader.Application.DownloadHistory.GetDownloadHistory
{
    public class GetAudioDownloadHistoryQuery : SearchCriteria, IQuery<Page<AudioDownloadHistoryDTO>>
    {
    }
}
