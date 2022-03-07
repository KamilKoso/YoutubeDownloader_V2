using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeVideoMetadata
{
    public class GetYoutubeVideoMetadataQuery : IQuery<YoutubeVideoMetadataDTO>
    {
        public string VideoUrl { get; set; }
    }
}
