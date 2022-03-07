using System;

namespace YoutubeDownloader.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, bool showToast = true)
            : base(message)
        {
            ShowToast = showToast;
        }

        public bool ShowToast { get; set; }
    }
}
