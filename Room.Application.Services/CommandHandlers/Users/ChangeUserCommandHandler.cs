using MediatR;
using Room.Application.Abstractions.Commands.Users;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.Users;

/// <summary>
/// Обработчик команды на изменение пользователя
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class ChangeUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeUserCommand>
{
    public async Task Handle(ChangeUserCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.Id);

        // Если пользователь не найден - вызываем исключение
        if (user == null) throw new UserNotFoundException();

        // Устанавливаем имя
        user.UserName = request.UserName;

        // Устанавливаем аватарку
        user.PhotoUrl = request.PhotoUrl;

        // Устанавливаем разрешения
        user.Allows = request.Allows;

        // Обновляем пользователя в репозитории
        await unitOfWork.UserRepository.Value.UpdateAsync(user);

        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}