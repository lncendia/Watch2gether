using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Room.YoutubeRoom.Ordering.Visitor;

public interface IYoutubeRoomSortingVisitor : ISortingVisitor<IYoutubeRoomSortingVisitor, Entities.YoutubeRoom>
{
    void Visit(OrderByLastActivityDate sorting);
}