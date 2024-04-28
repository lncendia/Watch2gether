using Films.Application.Abstractions.DTOs.Common;
using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.YoutubeRooms;
using Films.Application.Services.Extensions;
using Films.Application.Services.Mappers.Rooms;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Ordering;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Domain.Rooms.YoutubeRooms.Ordering;
using Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using Films.Domain.Rooms.YoutubeRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using MediatR;

namespace Films.Application.Services.QueryHandlers.YoutubeRooms;

public class SearchYoutubeRoomsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SearchYoutubeRoomsQuery, ListDto<YoutubeRoomShortDto>>
{
    public async Task<ListDto<YoutubeRoomShortDto>> Handle(SearchYoutubeRoomsQuery request,
        CancellationToken cancellationToken)
    {
        ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>? specification = null;

        if (request.OnlyPublic)
            specification.AddToSpecification(new OpenYoutubeRoomsSpecification());

        var order = new DescendingOrder<YoutubeRoom, IYoutubeRoomSortingVisitor>(new YoutubeRoomOrderByDate());

        var rooms = await unitOfWork.YoutubeRoomRepository.Value.FindAsync(specification, order, request.Skip,
            request.Take);

        if (rooms.Count == 0) return new ListDto<YoutubeRoomShortDto> { List = [], TotalCount = 0 };

        return new ListDto<YoutubeRoomShortDto>
        {
            List = rooms.Select(Mapper.Map).ToArray(),
            TotalCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(specification)
        };
    }
}