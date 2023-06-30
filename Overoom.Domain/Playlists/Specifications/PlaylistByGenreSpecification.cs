using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Playlists.Specifications;

public class PlaylistByGenreSpecification : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public PlaylistByGenreSpecification(string genre) => Genre = genre;

    public string Genre { get; }
    public bool IsSatisfiedBy(Playlist item) => item.Genres.Select(x=>x.ToLower()).Contains(Genre.ToLower());
    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}