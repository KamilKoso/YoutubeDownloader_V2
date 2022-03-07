using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YoutubeDownloader.Domain.History;

namespace YoutubeDownloader.Infrastructure.Database.Mappings
{
    public class AudioDownloadHistoryConfiguration : IEntityTypeConfiguration<AudioDownloadHistory>
    {
        public void Configure(EntityTypeBuilder<AudioDownloadHistory> builder)
        {
            builder.ToTable("AudioDownloadHistory");
            builder.Property(x => x.UserId).HasMaxLength(36);
            builder.Property(x => x.VideoId).HasMaxLength(11).IsRequired();
            builder.Property(x => x.VideoAuthor).HasMaxLength(30).IsRequired();
            builder.Property(x => x.VideoTitle).HasMaxLength(100).IsRequired();
        }
    }
}
