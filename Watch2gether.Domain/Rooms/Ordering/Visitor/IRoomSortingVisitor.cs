using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Rooms.Ordering.Visitor;

public interface IRoomSortingVisitor : ISortingVisitor<IRoomSortingVisitor, Room>
{
}