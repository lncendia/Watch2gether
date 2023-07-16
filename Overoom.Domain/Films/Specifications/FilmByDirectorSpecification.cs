using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByDirectorSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByDirectorSpecification(string director) => Director = director;

    public string Director { get; }
    public bool IsSatisfiedBy(Film item) => item.FilmTags.Directors.Any(x => x.ToUpper().Contains(Director.ToUpper()));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}