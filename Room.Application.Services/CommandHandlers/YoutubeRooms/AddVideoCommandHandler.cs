using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на добавление видео в очередь
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class AddVideoCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddVideoCommand>
{
    public async Task Handle(AddVideoCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);
        
        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Добавляем видео
        room.AddVideo(request.UserId, request.VideoUrl);
    }
}