using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Room.FilmRoom.Entities;
using Overoom.Domain.Room.FilmRoom.Ordering.Visitor;
using Overoom.Domain.Room.FilmRoom.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IFilmRoomRepository : IRepository<FilmRoom, Guid, IFilmRoomSpecificationVisitor, IFilmRoomSortingVisitor>
{
}