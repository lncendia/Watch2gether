using System.Linq.Expressions;
using Films.Domain.Comments.Entities;
using Films.Domain.Comments.Specifications;
using Films.Domain.Comments.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.Comment;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

public class CommentVisitor : BaseVisitor<CommentModel, ICommentSpecificationVisitor, Comment>, ICommentSpecificationVisitor
{
    protected override Expression<Func<CommentModel, bool>> ConvertSpecToExpression(ISpecification<Comment, ICommentSpecificationVisitor> spec)
    {
        var visitor = new CommentVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(FilmCommentsSpecification specification) => Expr = x => x.FilmId == specification.FilmId;

    public void Visit(UserCommentsSpecification specification) => Expr = x => x.UserId == specification.UserId;
}