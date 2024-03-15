using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.YoutubeRooms.ValueObjects;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на добавление видео в очередь
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class AddVideoCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<AddVideoCommand>
{
    public async Task Handle(AddVideoCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        // Добавляем видео
        room.AddVideo(request.ViewerId, new Video(request.VideoUrl));
    }
}