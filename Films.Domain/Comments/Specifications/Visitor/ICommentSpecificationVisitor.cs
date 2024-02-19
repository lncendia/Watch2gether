using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Comments.Specifications.Visitor;

public interface ICommentSpecificationVisitor : ISpecificationVisitor<ICommentSpecificationVisitor, Comment>
{
    void Visit(FilmCommentsSpecification specification);
    void Visit(UserCommentsSpecification specification);
}