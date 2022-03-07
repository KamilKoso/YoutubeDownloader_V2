using System.Threading.Tasks;

namespace YoutubeDownloader.Common.Dispatchers.EventDispatcher
{
    public interface IEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent @event) where TEvent : DomainEvent;
    }
}
