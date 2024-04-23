using System.Linq.Expressions;
using Room.Domain.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;
using Room.Infrastructure.Storage.Models.FilmRooms;

namespace Room.Infrastructure.Storage.Visitors.Specifications;

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
}