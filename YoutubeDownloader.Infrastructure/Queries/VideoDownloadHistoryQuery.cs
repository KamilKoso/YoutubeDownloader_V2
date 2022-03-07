using System.Threading.Tasks;
using YoutubeDownloader.Application.Queries.DownloadHistory;
using YoutubeDownloader.Domain.Common.Pagination;
using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Infrastructure.QueryBuilder;

namespace YoutubeDownloader.Infrastructure.Queries
{
    public class VideoDownloadHistoryQuery : IVideoDownloadHistoryQuery
    {
        private readonly SqlQueryBuilder queryBuilder;

        public VideoDownloadHistoryQuery(SqlQueryBuilder queryBuilder)
        {
            this.queryBuilder = queryBuilder;
        }

        public async Task<Page<VideoDownloadHistory>> GetVideoHistory(SearchCriteria searchCriteria, string userId)
        {
            return await queryBuilder
                            .Select("Id", "VideoTitle", "VideoId", "UserId", "QualityLabel", "BitrateInKilobytesPerSecond", "CreatedOn", "UpdatedOn")
                            .From("[dbo].[VideoDownloadHistory]")
                            .Where("UserId", userId)
                            .OrderBy(searchCriteria.OrderBy + (searchCriteria.IsAscending ? ", ASC" : ", DESC"))
                            .BuildPagedQuery<VideoDownloadHistory>(searchCriteria)
                            .Execute();
        }
    }
}
