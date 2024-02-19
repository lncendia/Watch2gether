using System.Collections.Generic;
using System.Linq;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.FilmRooms.Ordering.Visitor;

namespace Films.Domain.Rooms.FilmRooms.Ordering;

public class FilmRoomOrderByViewersCount : IOrderBy<FilmRoom, IFilmRoomSortingVisitor>
{
    public IEnumerable<FilmRoom> Order(IEnumerable<FilmRoom> items) => items.OrderBy(x => x.Viewers.Count);

    public IList<IEnumerable<FilmRoom>> Divide(IEnumerable<FilmRoom> items) =>
        Order(items).GroupBy(x => x.Viewers.Count).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IFilmRoomSortingVisitor visitor) => visitor.Visit(this);
}