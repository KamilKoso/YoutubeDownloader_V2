using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Domain.Repositories.Statistics;
using YoutubeDownloader.Infrastructure;
using YoutubeDownloader.Infrastructure.Database.Contexts;

namespace YoutubeDownloader.Infrastructure.Repositories.Statistics
{
    public class AudioDownloadHistoryRepository : Repository<AudioDownloadHistory>, IAudioDownloadHistoryRepository
    {
        public AudioDownloadHistoryRepository(YoutubeDownloaderContext context) : base(context)
        {

        }
    }
}
