using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmByNameSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByNameSpecification(string actor) => Name = actor;

    public string Name { get; }
    public bool IsSatisfiedBy(Film item) => item.FilmData.Name.Contains(Name);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}