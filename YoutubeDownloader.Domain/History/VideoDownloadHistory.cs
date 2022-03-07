
using EnsureThat;
using System;

namespace YoutubeDownloader.Domain.History
{
    public class VideoDownloadHistory : DownloadHistoryBase
    {
        public VideoDownloadHistory(string videoAuthor,
                                    string videoTitle,
                                    string videoId,
                                    string userId,
                                    string qualityLabel,
                                    double? bitrateInKilobytesPerSecond = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId);
            EnsureArg.IsNotEqualTo(userId, Guid.Empty.ToString());
            EnsureArg.IsNotNullOrWhiteSpace(videoId);
            EnsureArg.IsNotNullOrWhiteSpace(videoAuthor);
            EnsureArg.IsNotNullOrWhiteSpace(videoTitle);
            EnsureArg.IsNotNullOrWhiteSpace(qualityLabel);

            if(bitrateInKilobytesPerSecond is not null)
            {
                EnsureArg.IsInRange<double>((double)bitrateInKilobytesPerSecond, 1, double.MaxValue);
            }

            VideoAuthor = videoAuthor;
            VideoTitle = videoTitle;
            VideoId = videoId;
            UserId = userId;
            QualityLabel = qualityLabel;
            BitrateInKilobytesPerSecond = bitrateInKilobytesPerSecond;
        }

        public VideoDownloadHistory()
        {
            // EF Constructor
        }

        public string QualityLabel { get; set; }
        public double? BitrateInKilobytesPerSecond { get; set; }
    }
}
