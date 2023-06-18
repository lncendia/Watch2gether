using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Enums;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByTypeSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public readonly FilmType Type;
    public FilmByTypeSpecification(FilmType type) => Type = type;
    public bool IsSatisfiedBy(Film item) => item.Type == Type;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}