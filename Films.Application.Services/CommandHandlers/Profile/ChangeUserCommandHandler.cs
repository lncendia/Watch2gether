using Films.Application.Abstractions.Commands.Profile;
using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Profile;

public class ChangeUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeUserCommand>
{
    public async Task Handle(ChangeUserCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.Id);
        
        if (user == null) throw new UserNotFoundException();

        user.Username = request.UserName;
        user.PhotoUrl = request.PhotoUrl;

        await unitOfWork.UserRepository.Value.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}