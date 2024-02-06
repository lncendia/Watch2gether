using Films.Domain.Extensions;
using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByCountrySpecification(string country) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Country { get; } = country.GetUpper();

    public bool IsSatisfiedBy(Film item) => item.Countries.Any(x => x == Country);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}