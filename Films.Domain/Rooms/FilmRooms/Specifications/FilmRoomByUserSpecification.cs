using Films.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Specifications;

public class FilmRoomByUserSpecification(Guid userId) : ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>
{
    public Guid UserId { get; } = userId;

    public void Accept(IFilmRoomSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(FilmRoom item) => item.Viewers.Any(u => u == UserId);
}