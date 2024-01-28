using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByIdSpecification(Guid id) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public Guid Id { get; } = id;
    public bool IsSatisfiedBy(Film item) => item.Id == Id;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}