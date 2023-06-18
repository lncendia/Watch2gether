using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Playlists.Specifications;

public class PlaylistByNameSpecification : ISpecification<Entities.Playlist, IPlaylistSpecificationVisitor>
{
    public PlaylistByNameSpecification(string name) => Name = name;

    public string Name { get; }
    public bool IsSatisfiedBy(Entities.Playlist item) => item.Name == Name;

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}