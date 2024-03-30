using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Abstractions.Queries.YoutubeRooms;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.YoutubeRooms;
using MediatR;

namespace Films.Application.Services.QueryHandlers.YoutubeRooms;

public class YoutubeRoomByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<YoutubeRoomByIdQuery, YoutubeRoomDto>
{
    public async Task<YoutubeRoomDto> Handle(YoutubeRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.Id);
        if (room == null) throw new RoomNotFoundException();
        return Map(room, request.UserId);
    }

    private static YoutubeRoomDto Map(YoutubeRoom room, Guid? userId) => new()
    {
        Id = room.Id,
        ViewersCount = room.Viewers.Count,
        IsCodeNeeded = room.Viewers.All(v => v != userId) && !string.IsNullOrEmpty(room.Code),
        IsPrivate = !string.IsNullOrEmpty(room.Code),
        VideoAccess = room.VideoAccess
    };
}