using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmRooms;

public class FilmRoomByIdQuery : IRequest<FilmRoomDto>
{
    public required Guid Id { get; init; }
    public Guid? UserId { get; init; }
}