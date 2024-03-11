using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.FilmRooms;

/// <summary>
/// Обработчик команды на отключение от комнаты с фильмом
/// </summary>
public class DisconnectRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DisconnectRoomCommand>
{
    public async Task Handle(DisconnectRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);
        if (room == null) throw new RoomNotFoundException();

        room.Disconnect(request.UserId);

        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();
    }
}