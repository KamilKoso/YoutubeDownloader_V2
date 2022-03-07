using System;

namespace YoutubeDownloader.Application.DownloadHistory.GetVideoDownloadHistory
{
    public class VideoDownloadHistoryDTO
    {
        public int Id { get; set; }
        public string VideoId { get; set; }
        public string QualityLabel { get; set; }
        public double? BitrateInKilobytesPerSecond { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public YoutubeVideoMetadataDTO VideoMetadata { get; set; }
    }
}
