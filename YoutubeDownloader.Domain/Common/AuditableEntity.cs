using System;

namespace YoutubeDownloader.Domain.Common
{
    public class AuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
