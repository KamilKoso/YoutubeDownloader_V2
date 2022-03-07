using YoutubeDownloader.Domain.Access;
using YoutubeDownloader.Domain.Common;

namespace YoutubeDownloader.Domain.History
{
    public abstract class DownloadHistoryBase : TrackedEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string VideoAuthor { get; set; }
        public string VideoTitle { get; set; }
        public string VideoId { get; set; }

        public User User { get; set; }
    }
}
