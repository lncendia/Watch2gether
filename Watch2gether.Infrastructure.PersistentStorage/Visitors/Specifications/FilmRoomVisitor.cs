using System.Linq.Expressions;
using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Rooms.BaseRoom;
using Watch2gether.Domain.Rooms.FilmRoom;
using Watch2gether.Domain.Rooms.FilmRoom.Specifications;
using Watch2gether.Domain.Rooms.FilmRoom.Specifications.Visitor;
using Watch2gether.Domain.Specifications;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

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