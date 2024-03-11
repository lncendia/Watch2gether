using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.Rooms;

public class UserYoutubeRoomsQuery : IRequest<IReadOnlyCollection<YoutubeRoomDto>>
{
    public required Guid Id { get; init; }
}