using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rooms.FilmRoom.Specifications.Visitor;

public interface IFilmRoomSpecificationVisitor : ISpecificationVisitor<IFilmRoomSpecificationVisitor, FilmRoom>
{
    void Visit(RoomsByFilmSpecification specification);
    void Visit(OpenFilmsRoomsSpecification specification);
}