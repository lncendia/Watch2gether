using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Room.FilmRoom.Specifications.Visitor;

public interface IFilmRoomSpecificationVisitor : ISpecificationVisitor<IFilmRoomSpecificationVisitor, Entities.FilmRoom>
{
    void Visit(RoomsByFilmSpecification specification);
    void Visit(OpenFilmsRoomsSpecification specification);
}