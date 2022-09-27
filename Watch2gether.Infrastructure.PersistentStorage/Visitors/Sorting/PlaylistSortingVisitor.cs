using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Playlists;
using Watch2gether.Domain.Playlists.Ordering;
using Watch2gether.Domain.Playlists.Ordering.Visitor;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Playlists;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;

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