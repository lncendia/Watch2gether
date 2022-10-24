using Overoom.Infrastructure.PersistentStorage.Models.Rooms;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting.Models;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.YoutubeRoom;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;

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

    public void Visit(OrderByLastActivityDate sorting) =>
        SortItems.Add(new SortData<YoutubeRoomModel>(f => f.LastActivity, false));
}