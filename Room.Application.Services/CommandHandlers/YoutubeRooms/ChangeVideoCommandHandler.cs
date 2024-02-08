using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на смену текущего видео
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class ChangeVideoCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeVideoCommand>
{
    public async Task Handle(ChangeVideoCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);

        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Изменяем текущее видео
        room.ChangeVideo(request.UserId, request.VideoNumber);
    }
}