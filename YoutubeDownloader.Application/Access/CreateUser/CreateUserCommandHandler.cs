using System;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeDownloader.Common.Dispatchers.CommandDispatcher;
using YoutubeDownloader.Domain.Access;
using YoutubeDownloader.Domain.Repositories.Access;

namespace YoutubeDownloader.Application.Access.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandHandler(IUserRepository userRepository,
                                        IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User(command.Id, command.Name);
            userRepository.Add(user);
            await unitOfWork.Save();
        }
    }
}
