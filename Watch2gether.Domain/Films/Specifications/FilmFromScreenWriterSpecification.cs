using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmFromScreenWriterSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmFromScreenWriterSpecification(string screenWriter) => ScreenWriter = screenWriter;

    public string ScreenWriter { get; }
    public bool IsSatisfiedBy(Film item) => item.FilmData.Screenwriters.Any(x => x == ScreenWriter);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}