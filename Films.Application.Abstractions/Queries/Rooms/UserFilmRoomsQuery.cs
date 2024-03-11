using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.Rooms;

public class UserFilmRoomsQuery : IRequest<IReadOnlyCollection<FilmRoomDto>>
{
    public required Guid Id { get; init; }
}