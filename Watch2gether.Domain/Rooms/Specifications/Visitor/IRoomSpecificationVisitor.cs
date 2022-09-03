using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Rooms.Specifications.Visitor;

public interface IRoomSpecificationVisitor : ISpecificationVisitor<IRoomSpecificationVisitor, Room>
{
}