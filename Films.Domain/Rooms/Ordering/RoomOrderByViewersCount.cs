using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Ordering.Visitor;

namespace Films.Domain.Rooms.Ordering;

public class RoomOrderByViewersCount : IOrderBy<Room, IRoomSortingVisitor>
{
    public IEnumerable<Room> Order(IEnumerable<Room> items) => items.OrderBy(x => x.ViewersCount);

    public IList<IEnumerable<Room>> Divide(IEnumerable<Room> items) =>
        Order(items).GroupBy(x => x.OwnerId).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IRoomSortingVisitor visitor) => visitor.Visit(this);
}