using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmFromIdsSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public List<Guid> Ids { get; }
    public FilmFromIdsSpecification(List<Guid> ids) => Ids = ids;

    public bool IsSatisfiedBy(Film item) => Ids.Contains(item.Id);
    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}