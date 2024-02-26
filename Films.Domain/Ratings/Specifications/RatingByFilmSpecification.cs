using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Ratings.Specifications;

public class RatingByFilmSpecification(Guid filmId) : ISpecification<Rating, IRatingSpecificationVisitor>
{
    public Guid FilmId { get; } = filmId;

    public void Accept(IRatingSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Rating item) => item.FilmId == FilmId;
}