using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDownloader.Application.Youtube.GetYoutubeVideo.Events;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.Common.Constans;
using YoutubeDownloader.Common.Dispatchers.EventDispatcher;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;
using YoutubeDownloader.Domain.Exceptions;
using YoutubeDownloader.Infrastructure.Hubs;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeVideo
{
    public class GetYoutubeVideoHandler : IQueryHandler<GetYoutubeVideoQuery, FileStream>
    {
        private readonly YoutubeClient youtubeClient;
        private readonly IHubContext<MediaSendingHub, IMediaSendingsHub> mediaSendingHub;
        private readonly ICurrentUserService currentUserService;
        private readonly IEventDispatcher eventDispatcher;
        private readonly string outputFilePath = "wwwroot/media/youtube/video/";


        public GetYoutubeVideoHandler(HttpClient httpClient,
                                      IHubContext<MediaSendingHub, IMediaSendingsHub> mediaSendingHub,
                                      ICurrentUserService currentUserService,
                                      IEventDispatcher eventDispatcher)
        {
            youtubeClient = new YoutubeClient(httpClient);
            this.mediaSendingHub = mediaSendingHub;
            this.currentUserService = currentUserService;
            this.eventDispatcher = eventDispatcher;
            Directory.CreateDirectory(outputFilePath);
        }


        public async Task<FileStream> Handle(GetYoutubeVideoQuery query, CancellationToken cancellationToken)
        {
            try
            {
            var metadata = await youtubeClient.Videos.GetAsync(query.VideoUrl);
            string fileName = Guid.NewGuid().ToString() + ".mp4";
            await DownloadMedia(query, outputFilePath + fileName, cancellationToken);
            DispatchEventWhenProcessingFinished(metadata.Id.Value, metadata.Title, metadata.Author.Title, query.VideoQualityLabel, query.Bitrate);
            return new FileStream(outputFilePath + fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (ArgumentException)
            {
                throw new DomainException(ErrorMessages.InvalidYoutubeUrl);
            }
        }


        private async Task DownloadMedia(GetYoutubeVideoQuery query, string outputFilePath, CancellationToken cancellationToken)
        {
            var progress = new Progress<double>((progress) => ReportProcessingProgress(query.SignalRConnectionId, progress));
            var streamManifest = await youtubeClient
                                        .Videos
                                        .Streams
                                        .GetManifestAsync(query.VideoUrl, cancellationToken);

            var conversionRequest = new ConversionRequestBuilder(outputFilePath)
                                            .Build();
            IStreamInfo audioStream;
            if (query.Bitrate is not null)
            {
                audioStream = streamManifest.GetAudioOnlyStreams()
                                               .FirstOrDefault(x => (int)x.Bitrate.KiloBitsPerSecond == (int)query.Bitrate);

                if (audioStream is null) throw new DomainException(ErrorMessages.ProvidedBitrateNotFound);
            }
            else
            {
                audioStream = streamManifest.GetAudioStreams().GetWithHighestBitrate();
            }
            var videoStream = streamManifest.GetVideoOnlyStreams().FirstOrDefault(x => x.VideoQuality.Label == query.VideoQualityLabel);
            if(videoStream is null) throw new DomainException(ErrorMessages.ProvidedVideoQualityNotFound);


            var streamInfo = new IStreamInfo[] { audioStream, videoStream };
            await youtubeClient
                .Videos
                .DownloadAsync(streamInfo, conversionRequest, progress, cancellationToken);
        }

        private void ReportProcessingProgress(string connectionId, double progress)
        {
                mediaSendingHub.Clients.Client(connectionId).ReportProcessingProgress(progress);
        }

        private void DispatchEventWhenProcessingFinished(string videoId, string videoTitle, string videoAuthor, string qualityLabel, double? bitrateInKilobytesPerSecond)
        {
            var @event = new YoutubeVideoProcessedEvent(videoId, videoTitle, videoAuthor, currentUserService.UserId, bitrateInKilobytesPerSecond, qualityLabel);
            eventDispatcher.Dispatch(@event);
        }
    }
}
