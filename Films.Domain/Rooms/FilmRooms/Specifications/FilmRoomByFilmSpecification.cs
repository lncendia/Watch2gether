using Films.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Specifications;

public class FilmRoomByFilmSpecification(Guid userId) : ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>
{
    public Guid FilmId { get; } = userId;

    public void Accept(IFilmRoomSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(FilmRoom item) => item.FilmId == FilmId;
}