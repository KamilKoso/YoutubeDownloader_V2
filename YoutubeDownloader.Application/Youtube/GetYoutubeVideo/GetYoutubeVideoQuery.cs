using System.IO;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeVideo
{
    public class GetYoutubeVideoQuery : IQuery<FileStream>
    {
        public string VideoUrl { get; set; }
        public string VideoQualityLabel { get; set; }
        public string SignalRConnectionId { get; set; }
        public double? Bitrate { get; set; }
    }
}
