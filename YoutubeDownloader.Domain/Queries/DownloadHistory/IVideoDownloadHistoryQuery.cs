using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoutubeDownloader.Domain.Common.Pagination;
using YoutubeDownloader.Domain.History;

namespace YoutubeDownloader.Application.Queries.DownloadHistory
{
    public interface IVideoDownloadHistoryQuery
    {
        Task<Page<VideoDownloadHistory>> GetVideoHistory(SearchCriteria searchCriteria, string userId);
    }
}
