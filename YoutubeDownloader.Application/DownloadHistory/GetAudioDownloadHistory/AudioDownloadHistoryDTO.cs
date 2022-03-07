using System;

namespace YoutubeDownloader.Application.DownloadHistory.GetAudioDownloadHistory
{
    public class AudioDownloadHistoryDTO
    {
        public int Id { get; set; }
        public string VideoId { get; set; }
        public double BitrateInKilobytesPerSecond { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public YoutubeVideoMetadataDTO VideoMetadata { get; set; }
    }
}
