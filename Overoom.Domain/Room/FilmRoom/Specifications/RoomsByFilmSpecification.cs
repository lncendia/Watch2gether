using Overoom.Domain.Room.FilmRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Room.FilmRoom.Specifications;

public class RoomsByFilmSpecification : ISpecification<Entities.FilmRoom, IFilmRoomSpecificationVisitor>
{
    public RoomsByFilmSpecification(Guid filmId) => FilmId = filmId;

    public Guid FilmId { get; }

    public bool IsSatisfiedBy(Entities.FilmRoom item) => item.FilmId == FilmId;

    public void Accept(IFilmRoomSpecificationVisitor visitor) => visitor.Visit(this);
}