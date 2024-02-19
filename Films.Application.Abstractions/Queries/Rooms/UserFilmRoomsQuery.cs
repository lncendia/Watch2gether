using Films.Application.Abstractions.Queries.Rooms.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Rooms;

public class UserFilmRoomsQuery : IRequest<IReadOnlyCollection<FilmRoomDto>>
{
    public required Guid Id { get; init; }
}