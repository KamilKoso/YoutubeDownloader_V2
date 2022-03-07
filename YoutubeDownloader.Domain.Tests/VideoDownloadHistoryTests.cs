
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using YoutubeDownloader.Domain.History;

namespace YoutubeDownloader.Domain.Tests
{
    public class VideoDownloadHistoryTests
    {
        [Fact]
        public void VideoDownloadHistory_WhenCreatingNewVideoDownloadHistoryWithCorrectData_ThenVideoDownloadHistoryIsCreated()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";
            string videoId = "jNQXAC9IVRw";
            string qualityLabel = "1080p60";
            double bitrateInKilobytesPerSecond = 128.0126953125;
            // when
            var history = new VideoDownloadHistory(videoAuthor, videoTitle, videoId, userId, qualityLabel, bitrateInKilobytesPerSecond);
            var historyWithNulledBitrate = new VideoDownloadHistory(videoAuthor, videoTitle, videoId, userId, qualityLabel);

            // then
            history.ShouldNotBeNull();
            history.UserId.ShouldBe(userId);
            history.VideoAuthor.ShouldBe(videoAuthor);
            history.VideoTitle.ShouldBe(videoTitle);
            history.VideoId.ShouldBe(videoId);
            history.BitrateInKilobytesPerSecond.ShouldBe(bitrateInKilobytesPerSecond);
            
            historyWithNulledBitrate.BitrateInKilobytesPerSecond.ShouldBe(null);
        }

        [Fact]
        public void VideoDownloadHistory_WhenCreatingNewVideoDownloadHistoryWithIncorrectUserId_ThenExceptionIsThrown()
        {
            // given
            string nullUserId = null;
            string emptyUserId = "";
            string whitespaceUserId = " ";
            string emptyGuidUserId = Guid.Empty.ToString();

            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";
            string videoId = "jNQXAC9IVRw";
            string qualityLabel = "1080p60";

            // when then
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, videoId, nullUserId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, videoId, emptyUserId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, videoId, whitespaceUserId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, videoId, emptyGuidUserId, qualityLabel));
        }
        [Fact]
        public void VideoDownloadHistory_WhenCreatingNewVideoDownloadHistoryWithIncorrectVideoAuthor_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();

            string emptyVideoAuthor = "";
            string whitespaceVideoAuthor = " ";
            string nullVideoAuthor = null;

            string qualityLabel = "1080p60";
            string videoTitle = "Me at zoo";
            string videoId = "jNQXAC9IVRw";
            // when then
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(emptyVideoAuthor, videoTitle, videoId, userId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(whitespaceVideoAuthor, videoTitle, videoId, userId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(nullVideoAuthor, videoTitle, videoId, userId, qualityLabel));
        }

        [Fact]
        public void VideoDownloadHistory_WhenCreatingNewVideoDownloadHistoryWithIncorrectVideoTitle_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";

            string nullVideoTitle = null;
            string emptyVideoTitle = "";
            string whitespaceVideoTitle = " ";

            string qualityLabel = "1080p60";
            string videoId = "jNQXAC9IVRw";
            // when then
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, nullVideoTitle, videoId, userId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, emptyVideoTitle, videoId, userId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, whitespaceVideoTitle, videoId, userId, qualityLabel));
        }

        [Fact]
        public void VideoDownloadHistory_WhenCreatingNewVideoDownloadHistoryWithIncorrectQualityLabel_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";

            string videoId = "jNQXAC9IVRw";
            string emptyQualityLabel = "";
            string whitespaceQeualityLabel = " ";
            string nullQualityLabel = null;


            // when then
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, videoId, userId, emptyQualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, videoId, userId, whitespaceQeualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, videoId, userId, nullQualityLabel));

        }

        [Fact]
        public void VideoDownloadHistory_WhenCreatingNewVideoDownloadHistoryWithIncorrectVideoId_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";

            string emptyVideoId = "";
            string nullVideoId = null;
            string whitespaceVideoId = " ";

            string qualityLabel = "1080p60";
            // when then
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, emptyVideoId, userId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, nullVideoId, userId, qualityLabel));
            Should.Throw<ArgumentException>(() => new VideoDownloadHistory(videoAuthor, videoTitle, whitespaceVideoId, userId, qualityLabel));

        }
    }
}
