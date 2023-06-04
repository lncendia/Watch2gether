using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByIdSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public FilmByIdSpecification(Guid id) => Id = id;

    public Guid Id { get; }
    public bool IsSatisfiedBy(Film.Entities.Film item) => item.Id == Id;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}