using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;

namespace Overoom.Domain.Rooms.FilmRoom.Ordering;

public class OrderByLastActivityDate : IOrderBy<Entities.FilmRoom, IFilmRoomSortingVisitor>
{
    public IEnumerable<Entities.FilmRoom> Order(IEnumerable<Entities.FilmRoom> items) => items.OrderBy(x => x.LastActivity);

    public IList<IEnumerable<Entities.FilmRoom>> Divide(IEnumerable<Entities.FilmRoom> items) =>
        Order(items).GroupBy(x => x.LastActivity).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IFilmRoomSortingVisitor visitor) => visitor.Visit(this);
}