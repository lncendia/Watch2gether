using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на установку режима полного экрана
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class SetFullscreenCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SetFullscreenCommand>
{
    public async Task Handle(SetFullscreenCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);
        
        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Устанавливаем флаг нахождения в полноэкранном режиме
        room.SetPause(request.UserId, request.Fullscreen);
              
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}