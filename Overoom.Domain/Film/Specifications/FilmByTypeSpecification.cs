using Overoom.Domain.Film.Enums;
using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByTypeSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public readonly FilmType Type;
    public FilmByTypeSpecification(FilmType type) => Type = type;
    public bool IsSatisfiedBy(Film.Entities.Film item) => item.Type == Type;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}