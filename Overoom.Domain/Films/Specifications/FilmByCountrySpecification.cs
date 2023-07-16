using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByCountrySpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Country { get; }
    public FilmByCountrySpecification(string country) => Country = country;

    public bool IsSatisfiedBy(Film item) =>
        item.FilmTags.Countries.Any(x => string.Equals(x, Country, StringComparison.CurrentCultureIgnoreCase));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}