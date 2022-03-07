using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDownloader.Common.Constans;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;
using YoutubeDownloader.Domain.Exceptions;
using YoutubeExplode;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeVideoMetadata
{
    public class YoutubeVideoMetadataHandler : IQueryHandler<GetYoutubeVideoMetadataQuery, YoutubeVideoMetadataDTO>
    {
        private readonly HttpClient httpClient;

        public YoutubeVideoMetadataHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<YoutubeVideoMetadataDTO> Handle(GetYoutubeVideoMetadataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var client = new YoutubeClient(httpClient);
                var metadata = await client.Videos.GetAsync(query.VideoUrl, cancellationToken);
                var streams = await client.Videos.Streams.GetManifestAsync(query.VideoUrl, cancellationToken);
                var videoResolutions = streams.GetVideoOnlyStreams().Select(x => x.VideoQuality.Label).Distinct();
                var audioBitrates = streams.GetAudioOnlyStreams().Select(x => x.Bitrate.KiloBitsPerSecond).OrderByDescending(x => x);

                return new YoutubeVideoMetadataDTO(metadata.Author.Title,
                                                   metadata.Author.ChannelId,
                                                   metadata.Title,
                                                   metadata.Url,
                                                   metadata.Duration,
                                                   metadata.Thumbnails.LastOrDefault().Url,
                                                   metadata.Engagement.ViewCount,
                                                   videoResolutions,
                                                   audioBitrates);
            }
            catch (ArgumentException)
            {
                throw new DomainException(ErrorMessages.InvalidYoutubeUrl);
            }
        }
    }
}
