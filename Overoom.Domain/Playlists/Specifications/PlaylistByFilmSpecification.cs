using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Playlists.Specifications;

public class PlaylistByFilmSpecification : ISpecification<Entities.Playlist, IPlaylistSpecificationVisitor>
{
    public PlaylistByFilmSpecification(Guid id) => Id = id;

    public Guid Id { get; }
    public bool IsSatisfiedBy(Entities.Playlist item) => item.Films.Any(f => f == Id);

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}