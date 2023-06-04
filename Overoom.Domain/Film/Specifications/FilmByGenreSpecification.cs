using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByGenreSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public string Genre { get; }
    public FilmByGenreSpecification(string genre) => Genre = genre;

    public bool IsSatisfiedBy(Film.Entities.Film item) => item.FilmTags.Genres.Select(x => x.ToLower()).Contains(Genre.ToLower());

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}