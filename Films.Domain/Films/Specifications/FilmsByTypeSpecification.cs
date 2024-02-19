using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmsByTypeSpecification(bool isSerial) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public bool IsSerial { get; } = isSerial;
    public bool IsSatisfiedBy(Film item) => item.IsSerial == IsSerial;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}