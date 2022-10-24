using Overoom.Domain.Rooms.FilmRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rooms.FilmRoom.Specifications;

public class RoomsByFilmSpecification : ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>
{
    public RoomsByFilmSpecification(Guid filmId) => FilmId = filmId;

    public Guid FilmId { get; }

    public bool IsSatisfiedBy(FilmRoom item) => item.FilmId == FilmId;

    public void Accept(IFilmRoomSpecificationVisitor visitor) => visitor.Visit(this);
}