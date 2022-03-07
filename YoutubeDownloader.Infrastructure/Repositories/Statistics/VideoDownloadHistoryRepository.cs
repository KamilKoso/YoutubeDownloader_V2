using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Domain.Repositories.Statistics;
using YoutubeDownloader.Infrastructure;
using YoutubeDownloader.Infrastructure.Database.Contexts;

namespace YoutubeDownloader.Infrastructure.Repositories.Statistics
{
    public class VideoDownloadHistoryRepository : Repository<VideoDownloadHistory>, IVideoDownloadHistoryRepository
    {
        public VideoDownloadHistoryRepository(YoutubeDownloaderContext context) : base(context)
        {

        }
    }
}
