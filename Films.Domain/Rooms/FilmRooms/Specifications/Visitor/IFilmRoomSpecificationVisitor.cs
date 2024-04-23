using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Specifications.Visitor;

public interface IFilmRoomSpecificationVisitor : ISpecificationVisitor<IFilmRoomSpecificationVisitor, FilmRoom>
{
    void Visit(FilmRoomByUserSpecification spec);
    void Visit(OpenFilmRoomsSpecification spec);
    void Visit(FilmRoomByFilmSpecification spec);
}