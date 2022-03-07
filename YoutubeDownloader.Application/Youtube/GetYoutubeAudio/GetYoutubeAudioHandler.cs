using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDownloader.Application.Youtube.GetYoutubeAudio.Events;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.Common.Constans;
using YoutubeDownloader.Common.Dispatchers.EventDispatcher;
using YoutubeDownloader.Common.Dispatchers.QueryDispatcher;
using YoutubeDownloader.Domain.Exceptions;
using YoutubeDownloader.Infrastructure.Hubs;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeAudio
{
    public class GetYoutubeAudioHandler : IQueryHandler<GetYoutubeAudioQuery, FileStream>
    {
        private readonly YoutubeClient youtubeClient;
        private readonly IHubContext<MediaSendingHub, IMediaSendingsHub> mediaSendingHub;
        private readonly IEventDispatcher eventDispatcher;
        private readonly ICurrentUserService currentUserService;
        private readonly string outputFilePath = "wwwroot/media/youtube/audio/";

        public GetYoutubeAudioHandler(HttpClient httpClient,
                                      IHubContext<MediaSendingHub, IMediaSendingsHub> mediaSendingHub,
                                      IEventDispatcher eventDispatcher,
                                      ICurrentUserService currentUserService)
        {
            youtubeClient = new YoutubeClient(httpClient);
            this.mediaSendingHub = mediaSendingHub;
            this.eventDispatcher = eventDispatcher;
            this.currentUserService = currentUserService;
            Directory.CreateDirectory(outputFilePath);
        }

        public async Task<FileStream> Handle(GetYoutubeAudioQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var metadata = await youtubeClient.Videos.GetAsync(query.VideoUrl);
                string fileName = Guid.NewGuid().ToString() + ".mp3";

                await DownloadMedia(query, outputFilePath + fileName, cancellationToken);
                DispatchEventWhenProcessingFinished(metadata.Id.Value, metadata.Title, metadata.Author.Title, query.Bitrate);
                return new FileStream(outputFilePath + fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (ArgumentException)
            {
                throw new DomainException(ErrorMessages.InvalidYoutubeUrl);
            }
        }


        private async Task DownloadMedia(GetYoutubeAudioQuery query, string outputFilePath, CancellationToken cancellationToken)
        {
            var progress = new Progress<double>((progress) => ReportProcessingProgress(query.SignalRConnectionId, progress));
            var streamManifest = await youtubeClient
                                        .Videos
                                        .Streams
                                        .GetManifestAsync(query.VideoUrl, cancellationToken);

            var conversionRequest = new ConversionRequestBuilder(outputFilePath)
                                            .Build();

            var audioStream = streamManifest.GetAudioOnlyStreams()
                                            .FirstOrDefault(x => (int)x.Bitrate.KiloBitsPerSecond == (int)query.Bitrate);

            if (audioStream is null) throw new DomainException(ErrorMessages.ProvidedBitrateNotFound);

            var streamInfo = new IStreamInfo[] { audioStream };

            await youtubeClient
                .Videos
                .DownloadAsync(streamInfo, conversionRequest, progress, cancellationToken);
        }

        private void ReportProcessingProgress(string connectionId, double progress)
        {
            mediaSendingHub.Clients.Client(connectionId).ReportProcessingProgress(progress);
        }


        private void DispatchEventWhenProcessingFinished(string videoId, string videoTitle, string videoAuthor, double bitrateInKilobytesPerSecond)
        {
            var @event = new YoutubeAudioProcessedEvent(videoId, videoTitle, videoAuthor, currentUserService.UserId, bitrateInKilobytesPerSecond);
            eventDispatcher.Dispatch(@event);
        }
    }
}
