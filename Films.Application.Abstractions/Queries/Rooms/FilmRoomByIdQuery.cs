using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.Rooms;

public class FilmRoomByIdQuery : IRequest<FilmRoomDto>
{
    public required Guid Id { get; init; }
}