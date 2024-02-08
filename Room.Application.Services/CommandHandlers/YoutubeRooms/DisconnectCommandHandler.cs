using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на отключение от комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class DisconnectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DisconnectCommand>
{
    public async Task Handle(DisconnectCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);
        
        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Устанавливаем, что пользователь не онлайн
        room.SetOnline(request.UserId, false);
              
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}