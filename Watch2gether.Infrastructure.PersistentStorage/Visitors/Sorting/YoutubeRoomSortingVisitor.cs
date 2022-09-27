using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Rooms.YoutubeRoom;
using Watch2gether.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;

public class YoutubeRoomSortingVisitor : BaseSortingVisitor<YoutubeRoomModel, IYoutubeRoomSortingVisitor, YoutubeRoom>, IYoutubeRoomSortingVisitor
{
    protected override List<SortData<YoutubeRoomModel>> ConvertOrderToList(IOrderBy<YoutubeRoom, IYoutubeRoomSortingVisitor> spec)
    {
        var visitor = new YoutubeRoomSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}