using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.YoutubeRooms;

public class SearchYoutubeRoomsQuery : IRequest<(IReadOnlyCollection<YoutubeRoomShortDto> rooms, int count)>
{
    public Guid? UserId { get; init; }
    public bool OnlyPublic { get; init; }
    public bool OnlyMy { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}