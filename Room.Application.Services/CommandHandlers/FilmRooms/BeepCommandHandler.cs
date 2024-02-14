using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Common.Exceptions;
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

        // Получаем инициатора действия
        var initiator = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);

        // Если инициатор не найден - вызываем исключение
        if (initiator == null) throw new UserNotFoundException();
        
        // Получаем цель действия
        var target = await unitOfWork.UserRepository.Value.GetAsync(request.TargetId);
        
        // Если цель не найдена - вызываем исключение
        if (target == null) throw new UserNotFoundException();

        // Вызываем звуковой сигнал
        room.Beep(initiator, target);
    }
}