using MediatR;
using Room.Application.Abstractions.Commands.Users;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Users.Entities;

namespace Room.Application.Services.CommandHandlers.Users;

/// <summary>
/// Обработчик команды на добавление пользователя
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class AddUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddUserCommand>
{
    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        // Создаем пользователя
        var user = new User(request.Id)
        {
            UserName = request.UserName,
            PhotoUrl = request.PhotoUrl
        };

        // Добавляем в репозиторий
        await unitOfWork.UserRepository.Value.AddAsync(user);

        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}