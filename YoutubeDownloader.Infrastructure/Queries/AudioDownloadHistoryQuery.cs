using System.Threading.Tasks;
using YoutubeDownloader.Application.Queries.DownloadHistory;
using YoutubeDownloader.Domain.Common.Pagination;
using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Infrastructure.QueryBuilder;

namespace YoutubeDownloader.Infrastructure.Queries
{
    public class AudioDownloadHistoryQuery : IAudioDownloadHistoryQuery
    {
        private readonly SqlQueryBuilder queryBuilder;

        public AudioDownloadHistoryQuery(SqlQueryBuilder queryBuilder)
        {
            this.queryBuilder = queryBuilder;
        }

        public async Task<Page<AudioDownloadHistory>> GetAudioHistory(SearchCriteria searchCriteria, string userId)
        {
            return await queryBuilder
                            .Select("Id", "UserId", "VideoAuthor", "VideoTitle", "VideoId", "BitrateInKilobytesPerSecond", "CreatedOn", "UpdatedOn")
                            .From("[dbo].[AudioDownloadHistory]")
                            .Where("UserId", userId)
                            .OrderBy(searchCriteria.OrderBy + (searchCriteria.IsAscending ? ", ASC" : ", DESC"))
                            .BuildPagedQuery<AudioDownloadHistory>(searchCriteria)
                            .Execute();
        }

    }
}
