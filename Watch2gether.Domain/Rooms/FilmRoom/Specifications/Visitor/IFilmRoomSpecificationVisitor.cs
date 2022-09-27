using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Rooms.FilmRoom.Specifications.Visitor;

public interface IFilmRoomSpecificationVisitor : ISpecificationVisitor<IFilmRoomSpecificationVisitor, FilmRoom>
{
    void Visit(RoomsByFilmSpecification specification);
    void Visit(OpenFilmsRoomsSpecification specification);
}