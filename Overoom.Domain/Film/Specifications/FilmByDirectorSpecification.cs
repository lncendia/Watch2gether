using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByDirectorSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public FilmByDirectorSpecification(string director) => Director = director;

    public string Director { get; }
    public bool IsSatisfiedBy(Film.Entities.Film item) => item.FilmTags.Directors.Any(x => x == Director);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}