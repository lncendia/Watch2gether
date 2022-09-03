using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Rooms.Ordering.Visitor;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;

public class RoomSortingVisitor : BaseSortingVisitor<RoomModel, IRoomSortingVisitor, Room>, IRoomSortingVisitor
{
    protected override List<SortData<RoomModel>> ConvertOrderToList(IOrderBy<Room, IRoomSortingVisitor> spec)
    {
        var visitor = new RoomSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}