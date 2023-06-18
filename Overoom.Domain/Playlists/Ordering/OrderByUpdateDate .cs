using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Ordering.Visitor;

namespace Overoom.Domain.Playlists.Ordering;

public class OrderByUpdateDate : IOrderBy<Entities.Playlist, IPlaylistSortingVisitor>
{
    public IEnumerable<Entities.Playlist> Order(IEnumerable<Entities.Playlist> items) => items.OrderBy(x => x.Updated);

    public IList<IEnumerable<Entities.Playlist>> Divide(IEnumerable<Entities.Playlist> items) =>
        Order(items).GroupBy(x => x.Updated).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IPlaylistSortingVisitor visitor) => visitor.Visit(this);
}