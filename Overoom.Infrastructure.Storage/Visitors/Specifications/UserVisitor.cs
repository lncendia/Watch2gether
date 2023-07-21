using System.Linq.Expressions;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;
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

    public void Visit(UserByEmailSpecification specification) =>
        Expr = x => x.EmailNormalized == specification.Email.ToUpper();

    public void Visit(UserByIdSpecification specification) => Expr = x => x.Id == specification.Id;

    public void Visit(UserByIdsSpecification specification) => Expr = x => specification.Ids.Any(id => id == x.Id);

    public void Visit(UserByNameSpecification specification) =>
        Expr = x => x.NameNormalized.Contains(specification.Name.ToUpper());
}