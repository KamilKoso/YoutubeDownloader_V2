using Shouldly;
using System.Net;
using System.Text.Json;
using Xunit;
using YoutubeDownloader.Application.DownloadHistory;
using YoutubeDownloader.Common.Constans;
using YoutubeDownloader.Integration.Tests.Utility;

namespace YoutubeDownloader.Integration.Tests
{
    public class YoutubeDownloaderControllerTests : TestHostFixture
    {

        /* ------------ METADATA ------------ */
        [Fact]
        public async Task YoutubeDownloader_WhenGettingVideoMetadata_ThenMetadataIsReurned()
        {
            //given
            string videoUrl = "https://www.youtube.com/watch?v=jNQXAC9IVRw";
            string url = $"youtubedownloader/metadata?videoUrl={videoUrl}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = JsonSerializer
                                .Deserialize<YoutubeVideoMetadataDTO>(
                                        await httpResponse.Content.ReadAsStringAsync(),
                                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //then
            httpResponse.EnsureSuccessStatusCode();
            result.ShouldNotBeNull();
            result.VideoUrl.ShouldBe(videoUrl);
            result.Duration.ShouldBe(TimeSpan.FromSeconds(19));
            result.Author.ShouldBe("jawed");
            result.ChannelId.ShouldBe("UC4QobU6STFB0P71PMvOGN5A");
            result.Title.ShouldBe("Me at the zoo");
        }

        [Theory]
        [InlineData("https://www.youtube.com/watch?v=WRONG_YOUTUBE_ID")]
        [InlineData("https://www.WRONG_DOMAIN.com/watch?v=jNQXAC9IVRw")]
        [InlineData("WRONG_URL")]
        [InlineData("#$@#$@#$")]
        [InlineData("3434243242342")]
        public async Task YoutubeDownloader_WhenGettingVideoMetadataWithInvalidYoutubeUrl_ThenExceptionIsThrown(string videoUrl)
        {
            //given
            string url = $"youtubedownloader/metadata?videoUrl={videoUrl}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
                                                                (await httpResponse.Content.ReadAsStringAsync());

            //then
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            result.ShouldContainKeyAndValue("description", ErrorMessages.InvalidYoutubeUrl);
        }

        /* ------------ VIDEO ------------ */

        [Fact]
        public async Task YoutubeDownloader_WhenGettingVideo_ThenVideoIsReturned()
        {
            //given
            string videoUrl = "https://www.youtube.com/watch?v=jNQXAC9IVRw";
            string videoQuality = "144p";
            string dummyConnectionId = Guid.Empty.ToString();
            string url = $"youtubedownloader/get-video?videoUrl={videoUrl}&videoQualityLabel={videoQuality}&signalRConnectionId={dummyConnectionId}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = await httpResponse.Content.ReadAsByteArrayAsync();

            //then
            httpResponse.EnsureSuccessStatusCode();
            result.ShouldNotBeNull().ShouldNotBeEmpty();
            httpResponse.Content.Headers.ContentType.MediaType.ShouldBe("video/mp4");
        }

        [Theory]
        [InlineData("https://www.youtube.com/watch?v=WRONG_YOUTUBE_ID")]
        [InlineData("https://www.WRONG_DOMAIN.com/watch?v=jNQXAC9IVRw")]
        [InlineData("WRONG_URL")]
        [InlineData("#$@#$@#$")]
        [InlineData("3434243242342")]
        public async Task YoutubeDownloader_WhenGettingVideoWithInvalidYoutubeUrl_ThenExceptionIsThrown(string videoUrl)
        {
            //given
            string videoQuality = "144p";
            string dummyConnectionId = Guid.Empty.ToString();
            string url = $"youtubedownloader/get-video?videoUrl={videoUrl}&videoQualityLabel={videoQuality}&signalRConnectionId={dummyConnectionId}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
                                                                (await httpResponse.Content.ReadAsStringAsync());

            //then
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            result.ShouldContainKeyAndValue("description", ErrorMessages.InvalidYoutubeUrl);
        }

        [Theory]
        [InlineData("1080p")] // That quality does not exist on this video
        [InlineData("60")] 
        [InlineData("abcd")] 
        [InlineData("#$@#$%#%#")] 
        public async Task YoutubeDownloader_WhenGettingVideoWithWrongQuality_ThenExceptionIsThrown(string videoQuality)
        {
            //given
            string videoUrl = "https://www.youtube.com/watch?v=jNQXAC9IVRw";
            string dummyConnectionId = Guid.Empty.ToString();
            string url = $"youtubedownloader/get-video?videoUrl={videoUrl}&videoQualityLabel={videoQuality}&signalRConnectionId={dummyConnectionId}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
                                                               (await httpResponse.Content.ReadAsStringAsync());

            //then
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            result.ShouldContainKeyAndValue("description", ErrorMessages.ProvidedVideoQualityNotFound);
        }

        [Theory]
        [InlineData(60334)]
        [InlineData(-32444)]
        [InlineData(0)]
        public async Task YoutubeDownloader_WhenGettingVideoWithInvalidBitrate_ThenExceptionIsThrown(double bitrate)
        {
            //given
            string videoUrl = "https://www.youtube.com/watch?v=jNQXAC9IVRw";
            string videoQuality = "144p";
            string dummyConnectionId = Guid.NewGuid().ToString();
            string url = $"youtubedownloader/get-video?videoUrl={videoUrl}&videoQualityLabel={videoQuality}&signalRConnectionId={dummyConnectionId}&bitrate={bitrate}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
                                                               (await httpResponse.Content.ReadAsStringAsync());

            //then
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            result.ShouldContainKeyAndValue("description", ErrorMessages.ProvidedBitrateNotFound);
        }

        /* ------------ AUDIO ------------ */

        [Fact]
        public async Task YoutubeDownloader_WhenGettingAudio_ThenAudioIsReturned()
        {
            //given
            string videoUrl = "https://www.youtube.com/watch?v=jNQXAC9IVRw";
            int bitrate = 50;
            string dummyConnectionId = Guid.Empty.ToString();
            string url = $"youtubedownloader/get-audio?videoUrl={videoUrl}&bitrate={bitrate}&signalRConnectionId={dummyConnectionId}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = await httpResponse.Content.ReadAsByteArrayAsync();

            //then
            httpResponse.EnsureSuccessStatusCode();
            result.ShouldNotBeNull().ShouldNotBeEmpty();
            httpResponse.Content.Headers.ContentType.MediaType.ShouldBe("audio/mpeg");
        }

        [Theory]
        [InlineData("https://www.youtube.com/watch?v=WRONG_YOUTUBE_ID")]
        [InlineData("https://www.WRONG_DOMAIN.com/watch?v=jNQXAC9IVRw")]
        [InlineData("WRONG_URL")]
        [InlineData("#$@#$@#$")]
        [InlineData("3434243242342")]
        public async Task YoutubeDownloader_WhenGettingAudioWithInvalidYoutubeUrl_ThenExceptionIsThrown(string videoUrl)
        {
            //given
            int bitrate = 50;
            string dummyConnectionId = Guid.Empty.ToString();
            string url = $"youtubedownloader/get-audio?videoUrl={videoUrl}&bitrate={bitrate}&signalRConnectionId={dummyConnectionId}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
                                                                            (await httpResponse.Content.ReadAsStringAsync());

            //then
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            result.ShouldContainKeyAndValue("description", ErrorMessages.InvalidYoutubeUrl);
        }

        [Theory]
        [InlineData(60334)]
        [InlineData(-32444)]
        [InlineData(0)]
        public async Task YoutubeDownloader_WhenGettingAudioWithInvalidBitrate_ThenExceptionIsThrown(double bitrate)
        {
            //given
            string dummyConnectionId = Guid.Empty.ToString();
            string videoUrl = "https://www.youtube.com/watch?v=jNQXAC9IVRw";
            string url = $"youtubedownloader/get-audio?videoUrl={videoUrl}&bitrate={bitrate}&signalRConnectionId={dummyConnectionId}";

            //when
            var httpResponse = await Client.GetAsync(url);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
                                                                            (await httpResponse.Content.ReadAsStringAsync());

            //then
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            result.ShouldContainKeyAndValue("description", ErrorMessages.ProvidedBitrateNotFound);
        }
    }
}
