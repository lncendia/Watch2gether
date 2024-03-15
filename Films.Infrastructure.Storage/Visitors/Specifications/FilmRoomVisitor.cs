using System.Linq.Expressions;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.FilmRooms.Specifications;
using Films.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.FilmRooms;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

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

    public void Visit(FilmRoomByUserSpecification spec) => Expr = x => x.Viewers.Any(v => v.UserId == spec.UserId);

    public void Visit(OpenFilmRoomsSpecification specification) => Expr = x => x.Code != null;
}