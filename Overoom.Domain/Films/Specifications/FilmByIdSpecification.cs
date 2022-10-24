using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByIdSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByIdSpecification(Guid id) => Id = id;

    public Guid Id { get; }
    public bool IsSatisfiedBy(Film item) => item.Id == Id;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}