using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Overoom.Infrastructure.Storage.Models.YoutubeRoom;
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

    public void Visit(YoutubeRoomOrderByLastActivityDate sorting) =>
        SortItems.Add(new SortData<YoutubeRoomModel>(f => f.LastActivity, false));
}