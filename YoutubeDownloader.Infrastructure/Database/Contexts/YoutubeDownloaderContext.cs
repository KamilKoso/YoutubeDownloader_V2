using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.Domain.Access;
using YoutubeDownloader.Domain.Common;
using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Infrastructure.Database.Mappings;

namespace YoutubeDownloader.Infrastructure.Database.Contexts
{
    public class YoutubeDownloaderContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public YoutubeDownloaderContext() { }

        public YoutubeDownloaderContext(DbContextOptions<YoutubeDownloaderContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AudioDownloadHistory> AudioDownloadStatistic { get; set; }
        public DbSet<VideoDownloadHistory> VideoDownloadStatistic { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AudioDownloadHistoryConfiguration());
            builder.ApplyConfiguration(new VideoDownloadHistoryConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("YoutubeDownloader"));
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedOn = DateTimeOffset.UtcNow;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<TrackedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = DateTimeOffset.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}