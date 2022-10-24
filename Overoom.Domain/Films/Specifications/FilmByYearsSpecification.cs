using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByYearsSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByYearsSpecification(int minYear, int maxYear)
    {
        MinYear = minYear;
        MaxYear = maxYear;
    }

    public int MinYear { get; }
    public int MaxYear { get; }

    bool ISpecification<Film, IFilmSpecificationVisitor>.IsSatisfiedBy(Film item) =>
        item.Date.Year <= MaxYear && item.Date.Year >= MinYear;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}