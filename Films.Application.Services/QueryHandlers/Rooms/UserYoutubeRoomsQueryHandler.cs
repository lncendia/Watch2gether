using Films.Application.Abstractions.Queries.Rooms;
using Films.Application.Abstractions.Queries.Rooms.DTOs;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using Films.Domain.Servers.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Rooms;

public class UserYoutubeRoomsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UserYoutubeRoomsQuery, IReadOnlyCollection<YoutubeRoomDto>>
{
    public async Task<IReadOnlyCollection<YoutubeRoomDto>> Handle(UserYoutubeRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var roomsSpecification = new YoutubeRoomByUserSpecification(request.Id);
        var rooms = await unitOfWork.YoutubeRoomRepository.Value.FindAsync(roomsSpecification);

        var serversSpecification = new ServersByIdsSpecification(rooms.Select(x => x.ServerId));
        var servers = await unitOfWork.ServerRepository.Value.FindAsync(serversSpecification);

        List<YoutubeRoomDto> youtubeRooms = [];

        foreach (var room in rooms)
        {
            var server = servers.First(s => s.Id == room.ServerId);
            youtubeRooms.Add(Mapper.Map(room, server));
        }

        return youtubeRooms;
    }
}