using System.Linq.Expressions;
using Overoom.Infrastructure.PersistentStorage.Models.Users;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Specifications;

public class UserVisitor : BaseVisitor<UserModel, IUserSpecificationVisitor, User>, IUserSpecificationVisitor
{
    protected override Expression<Func<UserModel, bool>> ConvertSpecToExpression(
        ISpecification<User, IUserSpecificationVisitor> spec)
    {
        var visitor = new UserVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(UserByEmailSpecification specification) => Expr = x => x.Email == specification.Email;
    public void Visit(UserByIdSpecification specification) => Expr = x => x.Id == specification.Id;
}