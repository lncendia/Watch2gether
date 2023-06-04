using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlist.Entities;
using Overoom.Domain.Playlist.Ordering;
using Overoom.Domain.Playlist.Ordering.Visitor;
using Overoom.Infrastructure.Storage.Models;
using Overoom.Infrastructure.Storage.Models.Playlists;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

public class PlaylistSortingVisitor : BaseSortingVisitor<PlaylistModel, IPlaylistSortingVisitor, Playlist>,
    IPlaylistSortingVisitor
{
    public void Visit(OrderByUpdateDate order) => SortItems.Add(new SortData<PlaylistModel>(x => x.Updated, false));

    public void Visit(OrderByCountFilms order) =>
        SortItems.Add(new SortData<PlaylistModel>(x => x.FilmsList.Length, false));

    protected override List<SortData<PlaylistModel>> ConvertOrderToList(
        IOrderBy<Playlist, IPlaylistSortingVisitor> spec)
    {
        var visitor = new PlaylistSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}