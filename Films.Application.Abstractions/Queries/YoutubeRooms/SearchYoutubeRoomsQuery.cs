using Films.Application.Abstractions.DTOs.Common;
using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.YoutubeRooms;

public class SearchYoutubeRoomsQuery : IRequest<ListDto<YoutubeRoomShortDto>>
{
    public Guid? UserId { get; init; }
    public bool OnlyPublic { get; init; }
    public bool OnlyMy { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}