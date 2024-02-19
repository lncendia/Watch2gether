using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.FilmRooms;

/// <summary>
/// Обработчик команды на отключение от комнаты с фильмом
/// </summary>
public class DisconnectFilmRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DisconnectFilmRoomCommand>
{
    public async Task Handle(DisconnectFilmRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);
        if (room == null) throw new RoomNotFoundException();

        room.Disconnect(request.UserId);

        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();
    }
}