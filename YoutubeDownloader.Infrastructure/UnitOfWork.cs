using System;
using System.Threading.Tasks;
using EnsureThat;
using YoutubeDownloader.Common;
using YoutubeDownloader.Infrastructure.Database.Contexts;
using YoutubeDownloader.Infrastructure.Exceptions;

namespace YoutubeDownloader.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoutubeDownloaderContext _context;

        public UnitOfWork(YoutubeDownloaderContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));

            _context = context;
        }

        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex.InnerException);
            }
        }
    }
}
