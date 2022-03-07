using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeDownloader.Common.Dispatchers.EventDispatcher;
using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Domain.Repositories.Statistics;

namespace YoutubeDownloader.Application.Youtube.GetYoutubeVideo.Events
{
    public class UpdateStatisticsWhenVideoProcessedHandler : IEventHandler<YoutubeVideoProcessedEvent>
    {
        private readonly IVideoDownloadHistoryRepository videoDownloadHistoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateStatisticsWhenVideoProcessedHandler(IVideoDownloadHistoryRepository videoDownloadHistoryRepository,
                                                         IUnitOfWork unitOfWork)
        {
            this.videoDownloadHistoryRepository = videoDownloadHistoryRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(YoutubeVideoProcessedEvent @event)
        {
            var history = new VideoDownloadHistory(@event.VideoAuthor,
                                                   @event.VideoTitle,
                                                   @event.VideoId,
                                                   @event.UserId,
                                                   @event.QualityLabel,
                                                   @event.BitrateInKilobytesPerSecond);

            videoDownloadHistoryRepository.Add(history);
            await unitOfWork.Save();
        }
    }
}
