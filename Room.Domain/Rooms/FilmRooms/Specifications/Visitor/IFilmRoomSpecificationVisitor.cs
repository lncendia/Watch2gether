using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Rooms.FilmRooms.Specifications.Visitor;

public interface IFilmRoomSpecificationVisitor : ISpecificationVisitor<IFilmRoomSpecificationVisitor, FilmRoom>;