using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.YoutubeRooms;
using Films.Application.Services.Mappers.Rooms;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Ordering;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Domain.Rooms.YoutubeRooms.Ordering;
using Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.YoutubeRooms;

public class UserYoutubeRoomsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UserYoutubeRoomsQuery, IReadOnlyCollection<YoutubeRoomShortDto>>
{
    public async Task<IReadOnlyCollection<YoutubeRoomShortDto>> Handle(UserYoutubeRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var order = new DescendingOrder<YoutubeRoom, IYoutubeRoomSortingVisitor>(new YoutubeRoomOrderByDate());

        var rooms = await unitOfWork.YoutubeRoomRepository.Value.FindAsync(
            new YoutubeRoomByUserSpecification(request.UserId), order);

        return rooms.Select(Mapper.Map).ToArray();
    }
}