using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

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
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        // Получаем инициатора действия
        var initiator = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);

        // Если инициатор не найден - вызываем исключение
        if (initiator == null) throw new UserNotFoundException();

        // Получаем цель действия
        var target = await unitOfWork.UserRepository.Value.GetAsync(request.TargetId);

        // Если цель не найдена - вызываем исключение
        if (target == null) throw new UserNotFoundException();

        // Меняем имя пользователю
        room.ChangeName(initiator, target, request.Name);
        
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}