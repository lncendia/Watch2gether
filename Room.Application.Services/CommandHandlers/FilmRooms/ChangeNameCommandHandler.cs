using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на изменение имени другому пользователю
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class ChangeNameCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<ChangeNameCommand>
{
    public async Task Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        // Меняем имя пользователю
        room.ChangeName(request.ViewerId, request.TargetId, request.Name);
        
        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}