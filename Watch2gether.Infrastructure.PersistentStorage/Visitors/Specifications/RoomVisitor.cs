using System.Linq.Expressions;
using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Rooms.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

public class RoomVisitor : BaseVisitor<RoomModel, IRoomSpecificationVisitor, Room>, IRoomSpecificationVisitor
{
    protected override Expression<Func<RoomModel, bool>> ConvertSpecToExpression(
        ISpecification<Room, IRoomSpecificationVisitor> spec)
    {
        var visitor = new RoomVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }
}