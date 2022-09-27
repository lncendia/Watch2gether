using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmByCountrySpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Country { get; }
    public FilmByCountrySpecification(string country) => Country = country;

    public bool IsSatisfiedBy(Film item) => item.FilmData.Countries.Contains(Country);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}