using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на установку таймлайна
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class SetTimeLineCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<SetTimeLineCommand>
{
    public async Task Handle(SetTimeLineCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        // Устанавливаем таймлайн
        room.SetTimeLine(request.ViewerId, request.TimeLine);
              
        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}