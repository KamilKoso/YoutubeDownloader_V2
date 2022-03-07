using YoutubeDownloader.Common.Dispatchers.EventDispatcher;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeAudio.Events
{
    public class YoutubeAudioProcessedEvent : DomainEvent
    {
        public YoutubeAudioProcessedEvent(string videoId,
                                          string videoTitle,
                                          string videoAuthor,
                                          string userId,
                                          double bitrateInKilobytesPerSecond)
        {
            VideoId = videoId;
            VideoTitle = videoTitle;
            VideoAuthor = videoAuthor;
            UserId = userId;
            BitrateInKilobytesPerSecond = bitrateInKilobytesPerSecond;
        }

        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string VideoAuthor { get; set; }
        public string UserId { get; set; }
        public double BitrateInKilobytesPerSecond { get; set; }
    }
}
