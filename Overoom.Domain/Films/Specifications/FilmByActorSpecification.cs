using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Films.Specifications;

public class FilmByActorSpecification : ISpecification<Film, IFilmSpecificationVisitor>
{
    public FilmByActorSpecification(string actor) => Actor = actor;

    public string Actor { get; }
    public bool IsSatisfiedBy(Film item) => item.FilmTags.Actors.Any(x => x.ActorName == Actor);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}