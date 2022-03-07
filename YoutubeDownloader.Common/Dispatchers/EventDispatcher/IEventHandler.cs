using System.Threading.Tasks;

namespace YoutubeDownloader.Common.Dispatchers.EventDispatcher
{
    public interface IEventHandler<in TEvent> where TEvent : DomainEvent
    {
        Task Handle(TEvent @event);
    }
}
