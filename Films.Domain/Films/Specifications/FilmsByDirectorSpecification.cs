using System.Linq;
using Films.Domain.Extensions;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmsByDirectorSpecification(string director) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Director { get; } = director.GetUpper();
    public bool IsSatisfiedBy(Film item) => item.Directors.Any(x => x.Contains(Director));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}