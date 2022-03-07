
using YoutubeDownloader.Common.Dispatchers.EventDispatcher;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeVideo.Events
{
    public class YoutubeVideoProcessedEvent : DomainEvent
    {
        public YoutubeVideoProcessedEvent(string videoId,
                                          string videoTitle,
                                          string videoAuthor,
                                          string userId,
                                          double? bitrateInKilobytesPerSecond,
                                          string qualityLabel)
        {
            VideoId = videoId;
            VideoTitle = videoTitle;
            VideoAuthor = videoAuthor;
            UserId = userId;
            BitrateInKilobytesPerSecond = bitrateInKilobytesPerSecond;
            QualityLabel = qualityLabel;
        }

        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string VideoAuthor { get; set; }
        public string UserId { get; set; }
        public double? BitrateInKilobytesPerSecond { get; set; }
        public string QualityLabel { get; set; }
    }
}
