using System.IO;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeAudio
{
    public class GetYoutubeAudioQuery : IQuery<FileStream>
    {
        public string VideoUrl { get; set; }
        public double Bitrate { get; set; }
        public string SignalRConnectionId { get; set; }
    }
}
