using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Overoom.Domain.Rooms.FilmRoom.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IFilmRoomRepository : IRepository<FilmRoom, Guid, IFilmRoomSpecificationVisitor, IFilmRoomSortingVisitor>
{
}