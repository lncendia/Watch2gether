using System.Linq.Expressions;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.User.Entities;
using Overoom.Domain.User.Specifications;
using Overoom.Domain.User.Specifications.Visitor;
using Overoom.Infrastructure.Storage.Models.User;

namespace Overoom.Infrastructure.Storage.Visitors.Specifications;

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