using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmByIdSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByIdSpecification(Guid id) => Id = id;

    public Guid Id { get; }
    public bool IsSatisfiedBy(Film item) => item.Id == Id;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}