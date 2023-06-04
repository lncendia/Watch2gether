using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications;

public class FilmByActorSpecification : ISpecification<Film.Entities.Film, IFilmSpecificationVisitor>
{
    public FilmByActorSpecification(string actor) => Actor = actor;

    public string Actor { get; }
    public bool IsSatisfiedBy(Film.Entities.Film item) => item.FilmTags.Actors.Any(x => x.ActorName == Actor);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}