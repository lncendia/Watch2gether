using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmsByYearsSpecification(int minYear, int maxYear) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public int MinYear { get; } = minYear;
    public int MaxYear { get; } = maxYear;

    bool ISpecification<Film, IFilmSpecificationVisitor>.IsSatisfiedBy(Film item) =>
        item.Year <= MaxYear && item.Year >= MinYear;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}