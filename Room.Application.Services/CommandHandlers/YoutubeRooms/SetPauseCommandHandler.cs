using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на установку паузы
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class SetPauseCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<SetPauseCommand>
{
    public async Task Handle(SetPauseCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        // Устанавливаем флаг нахождения на паузе
        room.SetPause(request.UserId, request.Pause);
              
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}