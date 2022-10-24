using System.Linq.Expressions;
using Overoom.Infrastructure.PersistentStorage.Models.Rooms;
using Overoom.Domain.Rooms;
using Overoom.Domain.Rooms.BaseRoom;
using Overoom.Domain.Rooms.FilmRoom;
using Overoom.Domain.Rooms.FilmRoom.Specifications;
using Overoom.Domain.Rooms.FilmRoom.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Specifications;

public class FilmRoomVisitor : BaseVisitor<FilmRoomModel, IFilmRoomSpecificationVisitor, FilmRoom>,
    IFilmRoomSpecificationVisitor
{
    protected override Expression<Func<FilmRoomModel, bool>> ConvertSpecToExpression(
        ISpecification<FilmRoom, IFilmRoomSpecificationVisitor> spec)
    {
        var visitor = new FilmRoomVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(OpenFilmsRoomsSpecification specification) => Expr = x => x.IsOpen == specification.IsOpen;
    public void Visit(RoomsByFilmSpecification specification) => Expr = x => x.FilmId == specification.FilmId;
}