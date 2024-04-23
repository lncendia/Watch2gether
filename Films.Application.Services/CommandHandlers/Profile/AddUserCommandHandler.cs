using Films.Application.Abstractions.Commands.Profile;
using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Users;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Profile;

public class AddUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddUserCommand>
{
    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.Id);

        if (user != null) throw new UserAlreadyExistsException();

        user = new User(request.Id)
        {
            Username = request.UserName,
            PhotoUrl = request.PhotoUrl
        };


        await unitOfWork.UserRepository.Value.AddAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}