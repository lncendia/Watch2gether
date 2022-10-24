using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByNameSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByNameSpecification(string name) => Name = name;

    public string Name { get; }
    public bool IsSatisfiedBy(Film item) => item.Name.Contains(Name);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}