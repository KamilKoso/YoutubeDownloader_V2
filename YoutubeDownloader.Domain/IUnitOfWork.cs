using System.Threading.Tasks;

namespace YoutubeDownloader.Common
{
    public interface IUnitOfWork
    {
        Task Save();
    }
}
