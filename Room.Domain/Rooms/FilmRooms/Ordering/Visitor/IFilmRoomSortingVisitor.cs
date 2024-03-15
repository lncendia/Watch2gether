using Room.Domain.Ordering.Abstractions;

namespace Room.Domain.Rooms.FilmRooms.Ordering.Visitor;

public interface IFilmRoomSortingVisitor : ISortingVisitor<IFilmRoomSortingVisitor, FilmRoom>;