using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByScreenWriterSpecification(string screenWriter) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string ScreenWriter { get; } = screenWriter;
    public bool IsSatisfiedBy(Film item) => item.Screenwriters.Any(x => x.Contains(ScreenWriter, StringComparison.CurrentCultureIgnoreCase));
    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}