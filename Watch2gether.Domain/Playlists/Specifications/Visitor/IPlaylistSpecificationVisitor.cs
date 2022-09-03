using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Playlists.Specifications.Visitor;

public interface IPlaylistSpecificationVisitor : ISpecificationVisitor<IPlaylistSpecificationVisitor, Playlist>
{
    void Visit(PlaylistFromFilmSpecification specification);
    void Visit(PlaylistFromNameSpecification specification);
}