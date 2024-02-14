using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на установку режима полного экрана
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class SetFullscreenCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<SetFullscreenCommand>
{
    public async Task Handle(SetFullscreenCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        // Устанавливаем флаг нахождения в полноэкранном режиме
        room.SetPause(request.UserId, request.Fullscreen);
              
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}