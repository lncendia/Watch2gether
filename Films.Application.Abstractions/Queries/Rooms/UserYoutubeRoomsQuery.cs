using Films.Application.Abstractions.Queries.Rooms.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Rooms;

public class UserYoutubeRoomsQuery : IRequest<IReadOnlyCollection<YoutubeRoomDto>>
{
    public required Guid Id { get; init; }
}