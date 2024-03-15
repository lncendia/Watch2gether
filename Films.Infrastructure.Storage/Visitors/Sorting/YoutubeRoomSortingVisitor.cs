using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Domain.Rooms.YoutubeRooms.Ordering;
using Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;
using Films.Infrastructure.Storage.Models.YoutubeRoom;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

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

    public void Visit(YoutubeRoomOrderByViewersCount order) =>
        SortItems.Add(new SortData<YoutubeRoomModel>(f => f.Viewers.Count, false));
}