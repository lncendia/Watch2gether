using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmRooms;

public class UserFilmRoomsQuery : IRequest<IReadOnlyCollection<FilmRoomShortDto>>
{
    public required Guid UserId { get; init; }
}