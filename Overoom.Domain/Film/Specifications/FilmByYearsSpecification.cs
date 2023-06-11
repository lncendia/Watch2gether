using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByYearsSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public FilmByYearsSpecification(int minYear, int maxYear)
    {
        MinYear = minYear;
        MaxYear = maxYear;
    }

    public int MinYear { get; }
    public int MaxYear { get; }

    bool ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>.IsSatisfiedBy(Film.Entities.Film item) =>
        item.Year <= MaxYear && item.Year >= MinYear;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}