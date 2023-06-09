using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Room.YoutubeRoom.Entities;
using Overoom.Domain.Room.YoutubeRoom.Ordering;
using Overoom.Domain.Room.YoutubeRoom.Ordering.Visitor;
using Overoom.Infrastructure.Storage.Models.Rooms;
using Overoom.Infrastructure.Storage.Models.Rooms.YoutubeRoom;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

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