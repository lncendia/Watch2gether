using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Ordering.Visitor;

namespace Overoom.Domain.Playlists.Ordering;

public class OrderByCountFilms : IOrderBy<Entities.Playlist, IPlaylistSortingVisitor>
{
    public IEnumerable<Entities.Playlist> Order(IEnumerable<Entities.Playlist> items) => items.OrderBy(x => x.Films.Count);

    public IList<IEnumerable<Entities.Playlist>> Divide(IEnumerable<Entities.Playlist> items) =>
        Order(items).GroupBy(x => x.Films.Count).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IPlaylistSortingVisitor visitor) => visitor.Visit(this);
}