using MediatR;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на исключение пользователя из комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class KickCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<KickCommand>
{
    public async Task Handle(KickCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);
        
        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();
        
        // Исключаем зрителя
        room.Kick(request.UserId, request.TargetId);
              
        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}