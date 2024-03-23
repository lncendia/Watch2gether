using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmRooms;

public class SearchFilmRoomsQuery : IRequest<(IReadOnlyCollection<FilmRoomDto> rooms, int count)>
{
    public Guid? UserId { get; init; }
    public Guid? FilmId { get; init; }
    public bool OnlyPublic { get; init; }
    public bool OnlyMy { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}