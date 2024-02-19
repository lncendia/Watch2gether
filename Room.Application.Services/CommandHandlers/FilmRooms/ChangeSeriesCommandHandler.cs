using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на изменение номера сезона и серии
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class ChangeSeriesCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<ChangeSeriesCommand>
{
    public async Task Handle(ChangeSeriesCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        // Меняем сезон и эпизод
        room.ChangeSeries(request.ViewerId, request.Season, request.Series);
              
        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}