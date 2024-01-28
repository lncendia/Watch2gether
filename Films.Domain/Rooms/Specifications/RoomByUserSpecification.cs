using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.Specifications;

public class RoomByUserSpecification : ISpecification<Room, IRoomSpecificationVisitor>
{
    public required Guid UserId { get; init; }

    public void Accept(IRoomSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Room item) => item.OwnerId == UserId;
}