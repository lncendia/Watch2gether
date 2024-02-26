using Films.Application.Abstractions.Commands.Users;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Users.ValueObjects;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Users;

public class ChangeUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeUserCommand>
{
    public async Task Handle(ChangeUserCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.Id);
        
        if (user == null) throw new UserNotFoundException();

        user.UserName = request.UserName;
        user.PhotoUrl = request.PhotoUrl;

        await unitOfWork.UserRepository.Value.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}