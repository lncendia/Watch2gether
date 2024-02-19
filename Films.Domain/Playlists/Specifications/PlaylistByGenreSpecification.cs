using System.Linq;
using Films.Domain.Extensions;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Playlists.Specifications;

public class PlaylistByGenreSpecification(string genre) : ISpecification<Playlist, IPlaylistSpecificationVisitor>
{
    public string Genre { get; } = genre.GetUpper();

    public bool IsSatisfiedBy(Playlist item) => item.Genres.Any(g => g == Genre);

    public void Accept(IPlaylistSpecificationVisitor visitor) => visitor.Visit(this);
}