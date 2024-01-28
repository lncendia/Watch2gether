using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.Specifications;

public class OpenRoomsSpecification : ISpecification<Room, IRoomSpecificationVisitor>
{
    public void Accept(IRoomSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Room item) => item.IsOpen;
}