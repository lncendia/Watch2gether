using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByNameSpecification(string title) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Title { get; } = title;
    public bool IsSatisfiedBy(Film item) => item.Title.Contains(Title, StringComparison.CurrentCultureIgnoreCase);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}