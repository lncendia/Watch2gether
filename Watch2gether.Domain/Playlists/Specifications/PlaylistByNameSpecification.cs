using Watch2gether.Domain.Playlists.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Playlists.Specifications;

public class PlaylistByNameSpecification : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public PlaylistByNameSpecification(string name) => Name = name;

    public string Name { get; }
    public bool IsSatisfiedBy(Playlist item) => item.Name == Name;

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}