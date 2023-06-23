using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;

public interface IYoutubeRoomSortingVisitor : ISortingVisitor<IYoutubeRoomSortingVisitor, Entities.YoutubeRoom>
{
    void Visit(YoutubeRoomOrderByLastActivityDate sorting);
}