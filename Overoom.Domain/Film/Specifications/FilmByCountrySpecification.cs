using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByCountrySpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public string Country { get; }
    public FilmByCountrySpecification(string country) => Country = country;

    public bool IsSatisfiedBy(Film.Entities.Film item) => item.FilmTags.Countries.Contains(Country);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}