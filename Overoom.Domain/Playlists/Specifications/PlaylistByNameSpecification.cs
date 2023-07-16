using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Playlists.Specifications;

public class PlaylistByNameSpecification : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public PlaylistByNameSpecification(string name) => Name = name;

    public string Name { get; }
    public bool IsSatisfiedBy(Playlist item) => item.Name.ToUpper().Contains(Name.ToUpper());

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}