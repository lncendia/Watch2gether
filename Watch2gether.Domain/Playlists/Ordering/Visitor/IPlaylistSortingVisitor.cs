using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Playlists.Ordering.Visitor;

public interface IPlaylistSortingVisitor : ISortingVisitor<IPlaylistSortingVisitor, Playlist>
{
    void Visit(OrderByUpdateDate order);
    void Visit(OrderByCountFilms order);
}