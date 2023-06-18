using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Playlists.Ordering.Visitor;

public interface IPlaylistSortingVisitor : ISortingVisitor<IPlaylistSortingVisitor, Entities.Playlist>
{
    void Visit(OrderByUpdateDate order);
    void Visit(OrderByCountFilms order);
}