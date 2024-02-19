using System;
using Films.Domain.Comments.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Comments.Specifications;

public class FilmCommentsSpecification(Guid filmId) : ISpecification<Comment, ICommentSpecificationVisitor>
{
    public Guid FilmId { get; } = filmId;
    public bool IsSatisfiedBy(Comment item) => item.FilmId == FilmId;

    public void Accept(ICommentSpecificationVisitor visitor) => visitor.Visit(this);
}