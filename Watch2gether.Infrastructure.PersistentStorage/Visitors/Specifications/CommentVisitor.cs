using System.Linq.Expressions;
using Watch2gether.Domain.Comments;
using Watch2gether.Domain.Comments.Specifications;
using Watch2gether.Domain.Comments.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Comments;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

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