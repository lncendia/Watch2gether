using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Films.Domain.Rooms.FilmRooms.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IFilmRoomRepository : IRepository<FilmRoom, Guid, IFilmRoomSpecificationVisitor, IFilmRoomSortingVisitor>;