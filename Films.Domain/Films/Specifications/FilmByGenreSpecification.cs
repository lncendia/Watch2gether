using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByGenreSpecification(string genre) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Genre { get; } = genre;

    public bool IsSatisfiedBy(Film item) =>
        item.Genres.Any(g => string.Equals(g, Genre, StringComparison.CurrentCultureIgnoreCase));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}