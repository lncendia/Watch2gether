using Films.Domain.Extensions;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmsByActorSpecification(string actor) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public string Actor { get; } = actor.GetUpper();

    public bool IsSatisfiedBy(Film item) => item.Actors.Any(x => x.Name.Contains(Actor));

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}