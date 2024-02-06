using Films.Application.Abstractions.Commands.UserSettings;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using MediatR;

namespace Films.Application.Services.Commands.UserSettings;

public class ChangeAllowsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeAllowsCommand>
{
    public async Task Handle(ChangeAllowsCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        user.UpdateAllows(request.Beep, request.Scream, request.Change);
        
        await unitOfWork.UserRepository.Value.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}