using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Rooms.FilmRoom;
using Watch2gether.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Watch2gether.Domain.Rooms.FilmRoom.Specifications.Visitor;

namespace Watch2gether.Domain.Abstractions.Repositories;

public interface IFilmRoomRepository : IRepository<FilmRoom, Guid, IFilmRoomSpecificationVisitor, IFilmRoomSortingVisitor>
{
}