
using EnsureThat;
using System;

namespace YoutubeDownloader.Domain.History
{
    public class AudioDownloadHistory : DownloadHistoryBase
    {
        public AudioDownloadHistory(string userId,
                                    string videoAuthor,
                                    string videoTitle,
                                    string videoId,
                                    double bitrateInKilobytesPerSecond)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId);
            EnsureArg.IsNotEqualTo(userId, Guid.Empty.ToString());
            EnsureArg.IsNotNullOrWhiteSpace(videoId);
            EnsureArg.IsNotNullOrWhiteSpace(videoAuthor);
            EnsureArg.IsNotNullOrWhiteSpace(videoTitle);
            EnsureArg.IsInRange<double>(bitrateInKilobytesPerSecond, 1, double.MaxValue);

            UserId = userId;
            VideoAuthor = videoAuthor;
            VideoTitle = videoTitle;
            VideoId = videoId;
            BitrateInKilobytesPerSecond = bitrateInKilobytesPerSecond;
        }

        public AudioDownloadHistory()
        {
            // EF Constructor
        }
        public double BitrateInKilobytesPerSecond { get; set; }
    }
}
