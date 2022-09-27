using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

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
        item.FilmData.Year <= MaxYear && item.FilmData.Year >= MinYear;

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}