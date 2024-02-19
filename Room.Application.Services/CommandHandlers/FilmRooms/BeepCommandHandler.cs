using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на отправку звукового сигнала другому пользователю
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class BeepCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<BeepCommand>
{
    public async Task Handle(BeepCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        // Вызываем звуковой сигнал
        room.Beep(request.ViewerId, request.TargetId);
    }
}