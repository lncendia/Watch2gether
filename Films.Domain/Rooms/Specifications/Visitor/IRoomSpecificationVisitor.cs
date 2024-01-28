using Films.Domain.Rooms.Entities;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.Specifications.Visitor;

public interface IRoomSpecificationVisitor : ISpecificationVisitor<IRoomSpecificationVisitor, Room>
{
    void Visit(RoomByUserSpecification spec);
    void Visit(RoomByTypeSpecification spec);
    void Visit(OpenRoomsSpecification spec);
}