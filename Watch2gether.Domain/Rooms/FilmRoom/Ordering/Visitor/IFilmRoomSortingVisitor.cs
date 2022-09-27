using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Rooms.FilmRoom.Ordering.Visitor;

public interface IFilmRoomSortingVisitor : ISortingVisitor<IFilmRoomSortingVisitor, FilmRoom>
{
}