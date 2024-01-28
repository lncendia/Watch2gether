using Films.Domain.Films.Entities;
using Films.Domain.Films.Enums;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByTypeSpecification(FilmType type) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public readonly FilmType Type = type;
    public bool IsSatisfiedBy(Film item) => item.Type == Type;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}