using System.Linq.Expressions;
using Overoom.Domain.Comment.Entities;
using Overoom.Domain.Comment.Specifications;
using Overoom.Domain.Comment.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Models.Comment;

namespace Overoom.Infrastructure.Storage.Visitors.Specifications;

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