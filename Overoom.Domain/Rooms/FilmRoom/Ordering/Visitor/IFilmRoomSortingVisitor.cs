using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;

public interface IFilmRoomSortingVisitor : ISortingVisitor<IFilmRoomSortingVisitor, Entities.FilmRoom>
{
    void Visit(FilmRoomOrderByLastActivityDate sorting);
}