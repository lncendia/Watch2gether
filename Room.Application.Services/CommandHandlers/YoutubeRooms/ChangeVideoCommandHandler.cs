using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на смену текущего видео
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class ChangeVideoCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<ChangeVideoCommand>
{
    public async Task Handle(ChangeVideoCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        // Изменяем текущее видео
        room.ChangeVideo(request.UserId, request.VideoId);
    }
}