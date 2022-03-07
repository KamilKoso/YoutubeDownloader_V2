using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeDownloader.Common.Dispatchers.EventDispatcher;
using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Domain.Repositories.Statistics;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeAudio.Events
{
    public class UpdateStatisticsWhenAudioProcessedHandler : IEventHandler<YoutubeAudioProcessedEvent>
    {
        private readonly IAudioDownloadHistoryRepository audioDownloadHistoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateStatisticsWhenAudioProcessedHandler(IAudioDownloadHistoryRepository audioDownloadHistoryRepository,
                                                         IUnitOfWork unitOfWork)
        {
            this.audioDownloadHistoryRepository = audioDownloadHistoryRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(YoutubeAudioProcessedEvent @event)
        {
            var history = new AudioDownloadHistory(@event.UserId,
                                                   @event.VideoAuthor,
                                                   @event.VideoTitle,
                                                   @event.VideoId,
                                                   @event.BitrateInKilobytesPerSecond);

            audioDownloadHistoryRepository.Add(history);
            await unitOfWork.Save();
        }
    }
}
