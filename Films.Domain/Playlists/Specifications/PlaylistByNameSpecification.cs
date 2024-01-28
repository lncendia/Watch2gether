using Films.Domain.Playlists.Entities;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Playlists.Specifications;

public class PlaylistByNameSpecification(string name) : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public string Name { get; } = name;
    public bool IsSatisfiedBy(Playlist item) => item.Name.ToUpper().Contains(Name.ToUpper());

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}