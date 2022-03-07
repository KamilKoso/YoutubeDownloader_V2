using YoutubeDownloader.Common.Dispatchers.CommandDispatcher;

namespace YoutubeDownloader.Application.Access.CreateUser
{
    public class CreateUserCommand : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
