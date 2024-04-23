using Films.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Specifications;

public class OpenFilmRoomsSpecification : ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>
{
    public void Accept(IFilmRoomSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(FilmRoom item) => string.IsNullOrEmpty(item.Code);
}