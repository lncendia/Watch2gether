using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Ordering.Visitor;
using Films.Domain.Rooms.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IRoomRepository : IRepository<Room, Guid, IRoomSpecificationVisitor, IRoomSortingVisitor>;