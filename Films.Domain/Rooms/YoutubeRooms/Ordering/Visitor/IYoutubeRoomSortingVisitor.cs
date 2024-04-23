using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;

public interface IYoutubeRoomSortingVisitor : ISortingVisitor<IYoutubeRoomSortingVisitor, YoutubeRoom>
{
    void Visit(YoutubeRoomOrderByViewersCount order);
    void Visit(YoutubeRoomOrderByDate order);
}