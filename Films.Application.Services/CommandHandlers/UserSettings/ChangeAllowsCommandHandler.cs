using Films.Application.Abstractions.Commands.UserSettings;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Users.ValueObjects;
using MediatR;

namespace Films.Application.Services.CommandHandlers.UserSettings;

public class ChangeAllowsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeAllowsCommand>
{
    public async Task Handle(ChangeAllowsCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        user.Allows = new Allows
        {
            Beep = request.Beep,
            Scream = request.Scream,
            Change = request.Change
        };

        await unitOfWork.UserRepository.Value.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}