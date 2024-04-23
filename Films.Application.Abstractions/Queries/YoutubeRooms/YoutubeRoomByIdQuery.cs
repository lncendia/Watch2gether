using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Queries.YoutubeRooms;

public class YoutubeRoomByIdQuery : IRequest<YoutubeRoomDto>
{
    public required Guid Id { get; init; }
    public Guid? UserId { get; init; }
}