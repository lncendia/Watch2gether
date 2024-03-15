using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.YoutubeRooms;
using Room.Domain.Rooms.YoutubeRooms.Entities;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды создания комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class CreateRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoomCommand>
{
    public async Task Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        // Создаем комнату
        var room = new YoutubeRoom
        {
            VideoAccess = request.VideoAccess,
            Id = request.Id,
            Owner = new YoutubeViewer
            {
                Id = request.Owner.Id,
                Allows = request.Owner.Allows,
                PhotoUrl = request.Owner.PhotoUrl,
                Username = request.Owner.Nickname
            }
        };

        // Добавляем комнату в репозиторий
        await unitOfWork.YoutubeRoomRepository.Value.AddAsync(room);

        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}