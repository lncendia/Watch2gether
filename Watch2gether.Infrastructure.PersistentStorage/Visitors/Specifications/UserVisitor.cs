using System.Linq.Expressions;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Users;
using Watch2gether.Domain.Users.Specifications;
using Watch2gether.Domain.Users.Specifications.Visitor;
using Watch2gether.Infrastructure.PersistentStorage.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

public class UserVisitor : BaseVisitor<UserModel, IUserSpecificationVisitor, User>, IUserSpecificationVisitor
{
    protected override Expression<Func<UserModel, bool>> ConvertSpecToExpression(
        ISpecification<User, IUserSpecificationVisitor> spec)
    {
        var visitor = new UserVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(UserFromEmailSpecification specification) => Expr = x => x.Email == specification.Email;
    public void Visit(UserFromIdSpecification specification) => Expr = x => x.Id == specification.Id;
}