using Room.Domain.Ordering.Abstractions;
using Room.Domain.Rooms.YoutubeRooms;
using Room.Domain.Rooms.YoutubeRooms.Ordering.Visitor;
using Room.Infrastructure.Storage.Models.YoutubeRooms;
using Room.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Room.Infrastructure.Storage.Visitors.Sorting;

public class YoutubeRoomSortingVisitor : BaseSortingVisitor<YoutubeRoomModel, IYoutubeRoomSortingVisitor, YoutubeRoom>,
    IYoutubeRoomSortingVisitor
{
    protected override List<SortData<YoutubeRoomModel>> ConvertOrderToList(
        IOrderBy<YoutubeRoom, IYoutubeRoomSortingVisitor> spec)
    {
        var visitor = new YoutubeRoomSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}