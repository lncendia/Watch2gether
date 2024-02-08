using MediatR;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на изменение имени другому пользователю
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class ChangeNameCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeNameCommand>
{
    public async Task Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);

        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Получаем инициатора действия
        var initiator = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);

        // Если инициатор не найден - вызываем исключение
        if (initiator == null) throw new UserNotFoundException();

        // Получаем цель действия
        var target = await unitOfWork.UserRepository.Value.GetAsync(request.TargetId);

        // Если цель не найдена - вызываем исключение
        if (target == null) throw new UserNotFoundException();

        // Меняем имя пользователю
        room.ChangeName(initiator, target, request.Name);
        
        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}