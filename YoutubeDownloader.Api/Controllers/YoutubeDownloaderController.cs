using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using YoutubeDownloader.Application.Youtube.GetYoutubeVideoMetadata;
using YoutubeDownloader.Application.Youtube.GetYoutubeAudio;
using YoutubeDownloader.Application.Youtube.GetYoutubeVideo;
using YoutubeDownloader.Common.Dispatchers;
using YoutubeDownloader.Common.Dispatchers.CommandDispatcher;
using System.Threading;
using YoutubeDownloader.Api.Infrastructure.Cache;

namespace YoutubeDownloader.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YoutubeDownloaderController : ControllerBase, IDisposable
    {
        private readonly IQueryDispatcher queryDispatcher;
        private string _fileToDelete;
        private bool disposedValue;

        public YoutubeDownloaderController(IQueryDispatcher queryDispatcher)
        {
            EnsureArg.IsNotNull(queryDispatcher, nameof(queryDispatcher));

            this.queryDispatcher = queryDispatcher;
        }

        [HttpGet("metadata")]
        [Cached(3 * 60 * 60)] // 3 hours
        [AllowAnonymous]
        public async Task<IActionResult> GetVideoMetadata([FromQuery] GetYoutubeVideoMetadataQuery parameter)
        {
            var result = await queryDispatcher.Dispatch(parameter);
            return Ok(result);
        }

        [HttpGet("get-audio")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAudio([FromQuery] GetYoutubeAudioQuery parameter, CancellationToken cancellationToken)
        {
            var fileStream = await queryDispatcher.Dispatch(parameter, cancellationToken);
            _fileToDelete = fileStream.Name;
            return File(fileStream, "audio/mpeg");
        }



        [HttpGet("get-video")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideo([FromQuery] GetYoutubeVideoQuery parameter, CancellationToken cancellationToken)
        {
            var fileStream = await queryDispatcher.Dispatch(parameter, cancellationToken);
            _fileToDelete = fileStream.Name;
            return File(fileStream, "video/mp4");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_fileToDelete is not null)
                    {
                        System.IO.File.Delete(_fileToDelete);
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}