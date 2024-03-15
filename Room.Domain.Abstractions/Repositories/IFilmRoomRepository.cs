using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Room.Domain.Rooms.FilmRooms.Specifications.Visitor;

namespace Room.Domain.Abstractions.Repositories;

public interface IFilmRoomRepository : IRepository<FilmRoom, Guid, IFilmRoomSpecificationVisitor, IFilmRoomSortingVisitor>;