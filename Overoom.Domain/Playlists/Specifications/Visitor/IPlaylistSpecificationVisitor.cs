using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Playlists.Specifications.Visitor;

public interface IPlaylistSpecificationVisitor : ISpecificationVisitor<IPlaylistSpecificationVisitor, Playlist>
{
    void Visit(PlaylistByFilmSpecification specification);
    void Visit(PlaylistByNameSpecification specification);
}