using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByDirectorSpecification(string director) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Director { get; } = director;
    public bool IsSatisfiedBy(Film item) => item.Directors.Any(x => x.ToUpper().Contains(Director.ToUpper()));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}