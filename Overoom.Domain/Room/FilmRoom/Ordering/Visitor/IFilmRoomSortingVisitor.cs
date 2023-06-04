using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Room.FilmRoom.Ordering.Visitor;

public interface IFilmRoomSortingVisitor : ISortingVisitor<IFilmRoomSortingVisitor, Entities.FilmRoom>
{
    void Visit(OrderByLastActivityDate sorting);
}