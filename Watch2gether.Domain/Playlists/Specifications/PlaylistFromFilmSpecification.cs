using Watch2gether.Domain.Playlists.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Playlists.Specifications;

public class PlaylistFromFilmSpecification : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public PlaylistFromFilmSpecification(Guid id) => Id = id;

    public Guid Id { get; }
    public bool IsSatisfiedBy(Playlist item) => item.Films.Any(f => f == Id);

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}