using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications;

public class FilmByActorSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByActorSpecification(string actor) => Actor = actor;

    public string Actor { get; }
    public bool IsSatisfiedBy(Film item) => item.FilmData.Actors.Any(x => x.ActorName == Actor);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}