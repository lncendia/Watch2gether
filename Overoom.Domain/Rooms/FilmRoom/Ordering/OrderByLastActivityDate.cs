using Overoom.Domain.Films;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;

namespace Overoom.Domain.Rooms.FilmRoom.Ordering;

public class OrderByLastActivityDate : IOrderBy<FilmRoom, IFilmRoomSortingVisitor>
{
    public IEnumerable<FilmRoom> Order(IEnumerable<FilmRoom> items) => items.OrderBy(x => x.LastActivity);

    public IList<IEnumerable<FilmRoom>> Divide(IEnumerable<FilmRoom> items) =>
        Order(items).GroupBy(x => x.LastActivity).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IFilmRoomSortingVisitor visitor) => visitor.Visit(this);
}