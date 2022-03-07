using System;

namespace YoutubeDownloader.Domain.Common
{
    public class TrackedEntity
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
