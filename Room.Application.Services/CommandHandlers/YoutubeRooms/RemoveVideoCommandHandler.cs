using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на удаление видео из списка
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class RemoveVideoCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<RemoveVideoCommand>
{
    public async Task Handle(RemoveVideoCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);
        
        // Удаляем видео
        room.RemoveVideo(request.ViewerId, request.VideoId);
    }
}