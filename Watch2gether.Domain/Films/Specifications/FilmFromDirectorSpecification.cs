using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmFromDirectorSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmFromDirectorSpecification(string director) => Director = director;

    public string Director { get; }
    public bool IsSatisfiedBy(Film item) => item.FilmData.Directors.Any(x => x == Director);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}