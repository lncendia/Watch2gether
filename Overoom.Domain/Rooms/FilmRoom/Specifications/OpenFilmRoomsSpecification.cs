using Overoom.Domain.Rooms.FilmRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rooms.FilmRoom.Specifications;

public class OpenFilmsRoomsSpecification : ISpecification<Entities.FilmRoom, IFilmRoomSpecificationVisitor>
{
    public OpenFilmsRoomsSpecification(bool isOpen) => IsOpen = isOpen;

    public bool IsOpen { get; }

    public bool IsSatisfiedBy(Entities.FilmRoom item) => item.IsOpen == IsOpen;

    public void Accept(IFilmRoomSpecificationVisitor visitor) => visitor.Visit(this);
}