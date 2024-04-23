using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Playlists.Ordering.Visitor;

public interface IPlaylistSortingVisitor : ISortingVisitor<IPlaylistSortingVisitor, Playlist>
{
    void Visit(PlaylistOrderByUpdateDate order);
    void Visit(PlaylistOrderByCount order);
}