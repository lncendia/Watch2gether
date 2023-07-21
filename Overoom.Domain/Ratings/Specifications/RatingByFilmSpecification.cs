using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Ratings.Specifications;

public class RatingByFilmSpecification : ISpecification<Rating, IRatingSpecificationVisitor>
{
    public RatingByFilmSpecification(Guid filmId) => FilmId = filmId;
    public Guid FilmId { get; }

    public void Accept(IRatingSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Rating item) => item.FilmId == FilmId;
}