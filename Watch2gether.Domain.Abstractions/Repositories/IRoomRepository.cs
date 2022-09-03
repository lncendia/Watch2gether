using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Rooms.Ordering.Visitor;
using Watch2gether.Domain.Rooms.Specifications.Visitor;

namespace Watch2gether.Domain.Abstractions.Repositories;

public interface IRoomRepository : IRepository<Room, Guid, IRoomSpecificationVisitor, IRoomSortingVisitor>
{
}