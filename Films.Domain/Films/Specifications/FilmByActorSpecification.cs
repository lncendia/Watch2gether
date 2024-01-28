using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmByActorSpecification(string actor) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Actor { get; } = actor;

    public bool IsSatisfiedBy(Film item) =>
        item.Actors.Any(x => x.Name.Contains(Actor, StringComparison.CurrentCultureIgnoreCase));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}