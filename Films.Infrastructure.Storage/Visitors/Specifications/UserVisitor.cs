using System.Linq.Expressions;
using Films.Domain.Specifications.Abstractions;
using Films.Domain.Users.Entities;
using Films.Domain.Users.Specifications;
using Films.Domain.Users.Specifications.Visitor;
using Films.Infrastructure.Storage.Models.User;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

public class UserVisitor : BaseVisitor<UserModel, IUserSpecificationVisitor, User>, IUserSpecificationVisitor
{
    protected override Expression<Func<UserModel, bool>> ConvertSpecToExpression(
        ISpecification<User, IUserSpecificationVisitor> spec)
    {
        var visitor = new UserVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }
    

    public void Visit(UserByIdsSpecification spec) => Expr = x => spec.Ids.Any(id => id == x.Id);
}