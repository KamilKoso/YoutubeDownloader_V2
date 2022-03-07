using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDownloader.Application.DownloadHistory.GetAudioDownloadHistory;
using YoutubeDownloader.Application.DownloadHistory.GetVideoDownloadHistory;
using YoutubeDownloader.Application.Queries.DownloadHistory;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;
using YoutubeDownloader.Domain.Common.Pagination;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace YoutubeDownloader.Application.DownloadHistory.GetDownloadHistory
{
    public class GetAudioDownloadHistoryHandler : IQueryHandler<GetAudioDownloadHistoryQuery, Page<AudioDownloadHistoryDTO>>
    {
        private readonly IAudioDownloadHistoryQuery audioDownloadHistoryQuery;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<GetAudioDownloadHistoryHandler> logger;
        private YoutubeClient youtubeClient;

        public GetAudioDownloadHistoryHandler(IAudioDownloadHistoryQuery audioDownloadHistoryQuery,
                                              ICurrentUserService currentUserService,
                                              HttpClient httpClient,
                                              ILogger<GetAudioDownloadHistoryHandler> logger)
        {
            this.audioDownloadHistoryQuery = audioDownloadHistoryQuery;
            this.currentUserService = currentUserService;
            this.logger = logger;
            youtubeClient = new YoutubeClient(httpClient);

        }

        public async Task<Page<AudioDownloadHistoryDTO>> Handle(GetAudioDownloadHistoryQuery query, CancellationToken cancellationToken)
        {
            var histories = await audioDownloadHistoryQuery.GetAudioHistory(query, currentUserService.UserId);
            var historyDto = histories.Items.Select(x => new AudioDownloadHistoryDTO
            {
                Id = x.Id,
                BitrateInKilobytesPerSecond = x.BitrateInKilobytesPerSecond,
                CreatedOn = x.CreatedOn,
                VideoId = x.VideoId,
            }).ToList();

            var tasks = new List<Task>();
            foreach(var history in historyDto)
            {
                tasks.Add(Task.Run(async () => history.VideoMetadata = await GetVideoMetadata(history.VideoId, cancellationToken)));
            }
            Task.WaitAll(tasks.ToArray(), cancellationToken);

            return new Page<AudioDownloadHistoryDTO>
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
