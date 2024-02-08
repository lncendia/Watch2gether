using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на отправку скримера
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class ScreamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ScreamCommand>
{
    public async Task Handle(ScreamCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);
        
        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Получаем инициатора действия
        var initiator = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        
        // Если инициатор не найден - вызываем исключение
        if (initiator == null) throw new UserNotFoundException();
        
        // Получаем цель действия
        var target = await unitOfWork.UserRepository.Value.GetAsync(request.TargetId);
        
        // Если цель не найдена - вызываем исключение
        if (target == null) throw new UserNotFoundException();
        
        // Вызываем скример
        room.Scream(initiator, target);
    }
}