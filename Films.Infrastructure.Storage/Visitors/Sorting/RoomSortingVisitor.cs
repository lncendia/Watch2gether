using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Ordering;
using Films.Domain.Rooms.Ordering.Visitor;
using Films.Infrastructure.Storage.Models.Room;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

public class RoomSortingVisitor : BaseSortingVisitor<RoomModel, IRoomSortingVisitor, Room>,
    IRoomSortingVisitor
{
    protected override List<SortData<RoomModel>> ConvertOrderToList(
        IOrderBy<Room, IRoomSortingVisitor> spec)
    {
        var visitor = new RoomSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(RoomOrderByViewersCount order) =>
        SortItems.Add(new SortData<RoomModel>(f => f.ViewersCount, false));
}