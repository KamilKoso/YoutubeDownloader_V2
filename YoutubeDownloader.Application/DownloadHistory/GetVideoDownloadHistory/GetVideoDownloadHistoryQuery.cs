using YoutubeDownloader.Application.DownloadHistory.GetAudioDownloadHistory;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;
using YoutubeDownloader.Domain.Common.Pagination;

namespace YoutubeDownloader.Application.DownloadHistory.GetVideoDownloadHistory
{
    public class GetVideoDownloadHistoryQuery : SearchCriteria, IQuery<Page<VideoDownloadHistoryDTO>>
    {
    }
}
