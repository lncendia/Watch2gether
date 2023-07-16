using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByGenreSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Genre { get; }
    public FilmByGenreSpecification(string genre) => Genre = genre;

    public bool IsSatisfiedBy(Film item) =>
        item.FilmTags.Genres.Any(g => string.Equals(g, Genre, StringComparison.CurrentCultureIgnoreCase));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}