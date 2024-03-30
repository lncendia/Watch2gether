using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmRooms;

public class SearchFilmRoomsQuery : IRequest<(IReadOnlyCollection<FilmRoomShortDto> rooms, int count)>
{
    public Guid? FilmId { get; init; }
    public bool OnlyPublic { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}