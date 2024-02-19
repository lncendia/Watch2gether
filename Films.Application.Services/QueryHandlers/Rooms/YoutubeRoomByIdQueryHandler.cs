using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Queries.Rooms;
using Films.Application.Abstractions.Queries.Rooms.DTOs;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Rooms;

public class YoutubeRoomByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<YoutubeRoomByIdQuery, YoutubeRoomDto>
{
    public async Task<YoutubeRoomDto> Handle(YoutubeRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.Id);
        if (room == null) throw new RoomNotFoundException();

        var server = await unitOfWork.ServerRepository.Value.GetAsync(room.ServerId);
        if (server == null) throw new ServerNotFoundException();

        return Mapper.Map(room, server);
    }
}