using Films.Application.Abstractions.DTOs.Common;
using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmRooms;

public class SearchFilmRoomsQuery : IRequest<ListDto<FilmRoomShortDto>>
{
    public Guid? FilmId { get; init; }
    public bool OnlyPublic { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}