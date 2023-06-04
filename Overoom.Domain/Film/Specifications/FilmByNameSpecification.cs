using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByNameSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public FilmByNameSpecification(string name) => Name = name;

    public string Name { get; }
    public bool IsSatisfiedBy(Film.Entities.Film item) => item.Name.Contains(Name);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}