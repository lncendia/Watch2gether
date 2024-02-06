using Films.Domain.Extensions;
using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByNameSpecification(string title) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Title { get; } = title.GetUpper();
    public bool IsSatisfiedBy(Film item) => item.Title.Contains(Title);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}