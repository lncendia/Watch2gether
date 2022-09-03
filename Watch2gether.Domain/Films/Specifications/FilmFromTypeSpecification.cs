using Watch2gether.Domain.Films.Enums;
using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmFromTypeSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public readonly FilmType Type;
    public FilmFromTypeSpecification(FilmType type) => Type = type;
    public bool IsSatisfiedBy(Film item) => item.Type == Type;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}