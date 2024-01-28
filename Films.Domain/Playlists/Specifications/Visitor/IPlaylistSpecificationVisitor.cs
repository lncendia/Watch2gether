using Films.Domain.Playlists.Entities;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Playlists.Specifications.Visitor;

public interface IPlaylistSpecificationVisitor : ISpecificationVisitor<IPlaylistSpecificationVisitor, Playlist>
{
    void Visit(PlaylistByFilmSpecification specification);
    void Visit(PlaylistByNameSpecification specification);
    void Visit(PlaylistByGenreSpecification specification);
}