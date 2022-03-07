using YoutubeDownloader.Domain.Access;
using YoutubeDownloader.Domain.Repositories.Access;
using YoutubeDownloader.Infrastructure.Database.Contexts;

namespace YoutubeDownloader.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(YoutubeDownloaderContext context) : base(context)
        {

        }
    }
}
