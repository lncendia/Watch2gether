using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByIdsSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByIdsSpecification(IEnumerable<Guid> ids) => Ids = ids;

    public IEnumerable<Guid> Ids { get; }
    public bool IsSatisfiedBy(Film item) => Ids.Any(x => item.Id == x);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}