using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByIdsSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public FilmByIdsSpecification(IEnumerable<Guid> ids) => Ids = ids;

    public IEnumerable<Guid> Ids { get; }
    public bool IsSatisfiedBy(Film.Entities.Film item) => Ids.Any(x => item.Id == x);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}