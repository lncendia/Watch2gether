using System;
using System.Collections.Generic;
using System.Linq;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications;

public class FilmsByIdsSpecification(IEnumerable<Guid> ids) : ISpecification<Film, IFilmSpecificationVisitor>
{
    public IEnumerable<Guid> Ids { get; } = ids;
    public bool IsSatisfiedBy(Film item) => Ids.Any(x => item.Id == x);

    public void Accept(IFilmSpecificationVisitor visitor) => visitor.Visit(this);
}