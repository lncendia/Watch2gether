using System.Linq.Expressions;
using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Specifications;
using Films.Domain.Rooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.Room;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

public class RoomVisitor : BaseVisitor<RoomModel, IRoomSpecificationVisitor, Room>,
    IRoomSpecificationVisitor
{
    protected override Expression<Func<RoomModel, bool>> ConvertSpecToExpression(
        ISpecification<Room, IRoomSpecificationVisitor> spec)
    {
        var visitor = new RoomVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(RoomByUserSpecification spec) => Expr = x => x.OwnerId == spec.UserId;

    public void Visit(RoomByTypeSpecification spec) => Expr = x => x.Type == spec.Type;

    public void Visit(OpenRoomsSpecification specification) => Expr = x => x.IsOpen;
}