using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByScreenWriterSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public FilmByScreenWriterSpecification(string screenWriter) => ScreenWriter = screenWriter;

    public string ScreenWriter { get; }
    public bool IsSatisfiedBy(Film.Entities.Film item) => item.FilmTags.Screenwriters.Any(x => x == ScreenWriter);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}