using Watch2gether.Domain.Comments.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Comments.Specifications;

public class FilmCommentsSpecification : ISpecification<Comment, ICommentSpecificationVisitor>
{
    public FilmCommentsSpecification(Guid filmId) => FilmId = filmId;

    public Guid FilmId { get; }
    public bool IsSatisfiedBy(Comment item) => item.FilmId == FilmId;

    public void Accept(ICommentSpecificationVisitor visitor) => visitor.Visit(this);
}