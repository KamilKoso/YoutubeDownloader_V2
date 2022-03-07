using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDownloader.Application.DownloadHistory.GetAudioDownloadHistory;
using YoutubeDownloader.Application.Queries.DownloadHistory;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;
using YoutubeDownloader.Domain.Common.Pagination;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace YoutubeDownloader.Application.DownloadHistory.GetVideoDownloadHistory
{
    public class GetVideoDownloadHistoryHandler : IQueryHandler<GetVideoDownloadHistoryQuery, Page<VideoDownloadHistoryDTO>>
    {
        private readonly IVideoDownloadHistoryQuery videoDownloadHistoryQuery;
        private readonly ICurrentUserService currentUserService;
        private YoutubeClient youtubeClient;

        public GetVideoDownloadHistoryHandler(IVideoDownloadHistoryQuery videoDownloadHistoryQuery,
                                              ICurrentUserService currentUserService,
                                              HttpClient httpClient)
        {
            this.videoDownloadHistoryQuery = videoDownloadHistoryQuery;
            this.currentUserService = currentUserService;
            youtubeClient = new YoutubeClient(httpClient);

        }

        public async Task<Page<VideoDownloadHistoryDTO>> Handle(GetVideoDownloadHistoryQuery query, CancellationToken cancellationToken)
        {
            var histories = await videoDownloadHistoryQuery.GetVideoHistory(query, currentUserService.UserId);
            var historyDto = histories.Items.Select(x => new VideoDownloadHistoryDTO
            {
                Id = x.Id,
                BitrateInKilobytesPerSecond = x.BitrateInKilobytesPerSecond,
                QualityLabel = x.QualityLabel,
                CreatedOn = x.CreatedOn,
                VideoId = x.VideoId,
            }).ToList();

            var tasks = new List<Task>();
            foreach (var history in historyDto)
            {
                tasks.Add(Task.Run(async () => history.VideoMetadata = await GetVideoMetadata(history.VideoId, cancellationToken)));
            }
            Task.WaitAll(tasks.ToArray(), cancellationToken);

            return new Page<VideoDownloadHistoryDTO>
            {
                Items = historyDto,
                PageNumber = histories.PageNumber,
                PageSize = histories.PageSize,
                TotalCount = histories.TotalCount
            };
        }


        private async Task<YoutubeVideoMetadataDTO> GetVideoMetadata(string videoId, CancellationToken cancellationToken)
        {
            var metadata = await youtubeClient.Videos.GetAsync(videoId, cancellationToken);
            var streams = await youtubeClient.Videos.Streams.GetManifestAsync(videoId, cancellationToken);
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
    }
}
