using System.Linq;
using Films.Domain.Extensions;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmsByScreenWriterSpecification(string screenWriter) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string ScreenWriter { get; } = screenWriter.GetUpper();
    public bool IsSatisfiedBy(Film item) => item.Screenwriters.Any(x => x.Contains(ScreenWriter));
    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}