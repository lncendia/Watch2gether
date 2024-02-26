using Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.YoutubeRooms;

/// <summary>
/// Обработчик команды на отключение от комнаты ютуб
/// </summary>
public class DisconnectRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DisconnectRoomCommand>
{
    public async Task Handle(DisconnectRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);
        if (room == null) throw new RoomNotFoundException();

        room.Disconnect(request.UserId);

        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();
    }
}