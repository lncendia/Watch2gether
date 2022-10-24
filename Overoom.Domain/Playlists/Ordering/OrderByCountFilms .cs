using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Ordering.Visitor;

namespace Overoom.Domain.Playlists.Ordering;

public class OrderByCountFilms : IOrderBy<Playlist, IPlaylistSortingVisitor>
{
    public IEnumerable<Playlist> Order(IEnumerable<Playlist> items) => items.OrderBy(x => x.Films.Count);

    public IList<IEnumerable<Playlist>> Divide(IEnumerable<Playlist> items) =>
        Order(items).GroupBy(x => x.Films.Count).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IPlaylistSortingVisitor visitor) => visitor.Visit(this);
}