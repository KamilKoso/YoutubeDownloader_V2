using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using YoutubeDownloader.Domain.History;

namespace YoutubeDownloader.Domain.Tests
{
    public class AudioDownloadHistoryTests
    {
        [Fact]
        public void AudioDownloadHistory_WhenCreatingNewAudioDownloadHistoryWithCorrectData_ThenAudioDownloadHistoryIsCreated()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";
            string videoId = "jNQXAC9IVRw";
            double bitrateInKilobytesPerSecond = 128.0126953125;
            // when
            var history = new AudioDownloadHistory(userId, videoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond);

            // then
            history.ShouldNotBeNull();
            history.UserId.ShouldBe(userId);
            history.VideoAuthor.ShouldBe(videoAuthor);
            history.VideoTitle.ShouldBe(videoTitle);
            history.VideoId.ShouldBe(videoId);
            history.BitrateInKilobytesPerSecond.ShouldBe(bitrateInKilobytesPerSecond);
        }

        [Fact]
        public void AudioDownloadHistory_WhenCreatingNewAudioDownloadHistoryWithIncorrectUserId_ThenExceptionIsThrown()
        {
            // given
            string nullUserId = null;
            string emptyUserId = "";
            string whitespaceUserId = " ";
            string emptyGuidUserId = Guid.Empty.ToString();

            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";
            string videoId = "jNQXAC9IVRw";
            double bitrateInKilobytesPerSecond = 128.0126953125;
            // when then
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(nullUserId, videoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(emptyUserId, videoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(whitespaceUserId, videoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(emptyGuidUserId, videoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond));
        }
        [Fact]
        public void AudioDownloadHistory_WhenCreatingNewAudioDownloadHistoryWithIncorrectVideoAuthor_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();

            string emptyVideoAuthor = "";
            string whitespaceVideoAuthor = " ";
            string nullVideoAuthor = null;

            string videoTitle = "Me at zoo";
            string videoId = "jNQXAC9IVRw";
            double bitrateInKilobytesPerSecond = 128.0126953125;
            // when then
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, emptyVideoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, whitespaceVideoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, nullVideoAuthor, videoTitle, videoId, bitrateInKilobytesPerSecond));
        }

        [Fact]
        public void AudioDownloadHistory_WhenCreatingNewAudioDownloadHistoryWithIncorrectVideoTitle_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";

            string nullVideoTitle = null;
            string emptyVideoTitle = "";
            string whitespaceVideoTitle = " ";

            string videoId = "jNQXAC9IVRw";
            double bitrateInKilobytesPerSecond = 128.0126953125;
            // when then
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, nullVideoTitle, videoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, emptyVideoTitle, videoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, whitespaceVideoTitle, videoId, bitrateInKilobytesPerSecond));
        }

        [Fact]
        public void AudioDownloadHistory_WhenCreatingNewAudioDownloadHistoryWithIncorrectBitrate_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";

            string videoId = "jNQXAC9IVRw";

            double zeroBitrate = 0;
            double negativeBitrate = 0;
            double NaNBitrate = double.NaN;
            // when then
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, videoTitle, videoId, zeroBitrate));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, videoTitle, videoId, negativeBitrate));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, videoTitle, videoId, NaNBitrate));
        }

        [Fact]
        public void AudioDownloadHistory_WhenCreatingNewAudioDownloadHistoryWithIncorrectVideoId_ThenExceptionIsThrown()
        {
            // given
            string userId = Guid.NewGuid().ToString();
            string videoAuthor = "jawed";
            string videoTitle = "Me at zoo";

            string emptyVideoId = "";
            string nullVideoId = null;
            string whitespaceVideoId = " ";

            double bitrateInKilobytesPerSecond = 128.0126953125;
            // when then
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, videoTitle, emptyVideoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, videoTitle, nullVideoId, bitrateInKilobytesPerSecond));
            Should.Throw<ArgumentException>(() => new AudioDownloadHistory(userId, videoAuthor, videoTitle, whitespaceVideoId, bitrateInKilobytesPerSecond));
        }
    }
}
