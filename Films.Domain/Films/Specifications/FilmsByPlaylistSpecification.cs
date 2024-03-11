using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Playlists;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmsByPlaylistSpecification(Playlist playlist) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public Playlist Playlist { get; } = playlist;

    public bool IsSatisfiedBy(Film item) => Playlist.Films.Any(f => f == item.Id);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}