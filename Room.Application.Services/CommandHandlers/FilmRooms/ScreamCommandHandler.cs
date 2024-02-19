using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на отправку скримера
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class ScreamCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<ScreamCommand>
{
    public async Task Handle(ScreamCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        // Вызываем скример
        room.Scream(request.ViewerId, request.TargetId);
    }
}