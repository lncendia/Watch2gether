using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Playlists.Specifications.Visitor;

public interface IPlaylistSpecificationVisitor : ISpecificationVisitor<IPlaylistSpecificationVisitor, Entities.Playlist>
{
    void Visit(PlaylistByFilmSpecification specification);
    void Visit(PlaylistByNameSpecification specification);
}