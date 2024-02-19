using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Ordering.Visitor;

public interface IFilmRoomSortingVisitor : ISortingVisitor<IFilmRoomSortingVisitor, FilmRoom>
{
    void Visit(FilmRoomOrderByViewersCount order);
}