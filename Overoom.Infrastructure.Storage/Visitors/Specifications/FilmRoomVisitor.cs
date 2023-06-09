using System.Linq.Expressions;
using Overoom.Domain.Room.FilmRoom.Entities;
using Overoom.Domain.Room.FilmRoom.Specifications;
using Overoom.Domain.Room.FilmRoom.Specifications.Visitor;
using Overoom.Domain.Rooms;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Models;
using Overoom.Infrastructure.Storage.Models.Rooms;
using Overoom.Infrastructure.Storage.Models.Rooms.FilmRoom;

namespace Overoom.Infrastructure.Storage.Visitors.Specifications;

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