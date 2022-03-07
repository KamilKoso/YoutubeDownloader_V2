using System;
using System.Threading.Tasks;
using YoutubeDownloader.Domain.Common.Pagination;
using YoutubeDownloader.Domain.History;

namespace YoutubeDownloader.Application.Queries.DownloadHistory
{
    public interface IAudioDownloadHistoryQuery
    {
        Task<Page<AudioDownloadHistory>> GetAudioHistory(SearchCriteria searchCriteria, string userId);
    }
}
