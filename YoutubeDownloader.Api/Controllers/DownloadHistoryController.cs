using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YoutubeDownloader.Api.Infrastructure.Cache;
using YoutubeDownloader.Application.DownloadHistory.GetDownloadHistory;
using YoutubeDownloader.Application.DownloadHistory.GetVideoDownloadHistory;
using YoutubeDownloader.Common.Dispatchers;
using YoutubeDownloader.Common.Dispatchers.CommandDispatcher;

namespace YoutubeDownloader.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DownloadHistoryController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public DownloadHistoryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            EnsureArg.IsNotNull(queryDispatcher, nameof(queryDispatcher));
            EnsureArg.IsNotNull(commandDispatcher, nameof(commandDispatcher));

            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet("audio")]
        [Cached(30, CacheScope.User)]
        public async Task<IActionResult> GetAudioDownloadHistory([FromQuery] GetAudioDownloadHistoryQuery query)
        {
            var result = await queryDispatcher.Dispatch(query);
            return Ok(result);
        }

        [HttpGet("video")]
        [Cached(30, CacheScope.User)]
        public async Task<IActionResult> GetVideoDownloadHistory([FromQuery] GetVideoDownloadHistoryQuery query)
        {
            var result = await queryDispatcher.Dispatch(query);
            return Ok(result);
        }
    }
}
