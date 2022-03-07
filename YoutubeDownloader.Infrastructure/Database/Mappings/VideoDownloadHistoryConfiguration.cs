using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YoutubeDownloader.Domain.History;

namespace YoutubeDownloader.Infrastructure.Database.Mappings
{
    public class VideoDownloadHistoryConfiguration : IEntityTypeConfiguration<VideoDownloadHistory>
    {
        public void Configure(EntityTypeBuilder<VideoDownloadHistory> builder)
        {
            builder.ToTable("VideoDownloadHistory");
            builder.Property(x => x.UserId).HasMaxLength(36);
            builder.Property(x => x.VideoId).HasMaxLength(11).IsRequired();
            builder.Property(x => x.VideoAuthor).HasMaxLength(30).IsRequired();
            builder.Property(x => x.VideoTitle).HasMaxLength(100).IsRequired();
            builder.Property(x => x.QualityLabel).HasMaxLength(100).IsRequired();
        }
    }
}
