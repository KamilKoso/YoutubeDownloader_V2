using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YoutubeDownloader.Application.DownloadHistory
{
    public class YoutubeVideoMetadataDTO
    {
        public YoutubeVideoMetadataDTO(string author,
                                       string channelId,
                                       string title,
                                       string videoUrl,
                                       TimeSpan? duration,
                                       string thumbnailUrl,
                                       long views,
                                       IEnumerable<string> videoQualityLabels,
                                       IEnumerable<double> audioBitrates)
        {
            Author = author;
            ChannelId = channelId;
            Title = title;
            VideoUrl = videoUrl;
            Duration = duration;
            ThumbnailUrl = thumbnailUrl;
            Views = views;
            VideoQualityLabels = videoQualityLabels;
            BitratesKilobytesPerSecond = audioBitrates;
        }

        public YoutubeVideoMetadataDTO()
        {

        }

        public string Author { get; set; }
        public string ChannelId { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public TimeSpan? Duration { get; set; }
        public string ThumbnailUrl { get; set; }
        public long Views { get; set; }

        public IEnumerable<string> VideoQualityLabels { get; set; }
        public IEnumerable<double> BitratesKilobytesPerSecond { get; set; }
    }
}
