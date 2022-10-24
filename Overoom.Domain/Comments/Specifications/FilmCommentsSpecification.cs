using Overoom.Domain.Comments.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Comments.Specifications;

public class FilmCommentsSpecification : ISpecification<Comment, ICommentSpecificationVisitor>
{
    public FilmCommentsSpecification(Guid filmId) => FilmId = filmId;

    public Guid FilmId { get; }
    public bool IsSatisfiedBy(Comment item) => item.FilmId == FilmId;

    public void Accept(ICommentSpecificationVisitor visitor) => visitor.Visit(this);
}