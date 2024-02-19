using Films.Domain.Extensions;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Playlists.Specifications;

public class PlaylistByNameSpecification(string name) : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public string Name { get; } = name.GetUpper();
    public bool IsSatisfiedBy(Playlist item) => item.Name.Contains(Name);

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}