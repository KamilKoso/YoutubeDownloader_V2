using System;

namespace YoutubeDownloader.Common.Dispatchers.EventDispatcher
{
    public abstract class DomainEvent
    {
        public DateTime PublicationDate { get; }
        public bool IsDispatched { get; private set; }

        protected DomainEvent()
        {
            PublicationDate = DateTime.Now;
        }

        public void MarkAsDispatched()
        {
            IsDispatched = true;
        }
    }
}
