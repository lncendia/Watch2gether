using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.Entities;

namespace Films.Domain.Rooms.Ordering.Visitor;

public interface IRoomSortingVisitor : ISortingVisitor<IRoomSortingVisitor, Room>
{
    void Visit(RoomOrderByViewersCount order);
}