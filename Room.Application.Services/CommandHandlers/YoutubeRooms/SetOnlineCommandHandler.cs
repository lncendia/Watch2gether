using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на установку статуса подключения зрителя
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class SetOnlineCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<SetOnlineCommand>
{
    public async Task Handle(SetOnlineCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        // Устанавливаем, что пользователь не онлайн
        room.SetOnline(request.ViewerId, request.Online);
              
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}