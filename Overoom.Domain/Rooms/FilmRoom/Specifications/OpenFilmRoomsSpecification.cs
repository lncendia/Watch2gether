using Overoom.Domain.Rooms.FilmRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rooms.FilmRoom.Specifications;

public class OpenFilmsRoomsSpecification : ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>
{
    public OpenFilmsRoomsSpecification(bool isOpen) => IsOpen = isOpen;

    public bool IsOpen { get; }

    public bool IsSatisfiedBy(FilmRoom item) => item.IsOpen == IsOpen;

    public void Accept(IFilmRoomSpecificationVisitor visitor) => visitor.Visit(this);
}