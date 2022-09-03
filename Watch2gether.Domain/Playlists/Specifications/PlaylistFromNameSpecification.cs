using Watch2gether.Domain.Playlists.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Playlists.Specifications;

public class PlaylistFromNameSpecification : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public PlaylistFromNameSpecification(string name) => Name = name;

    public string Name { get; }
    public bool IsSatisfiedBy(Playlist item) => item.Name == Name;

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}