using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutubeDownloader.Web.Infrastructure
{
    public class AllowedContentSecurityPolicyHeader
    {
        public string[] StyleSources { get; set; }
        public string[] ImageSources { get; set; }
        public string[] FontSources { get; set; }
    }
}
