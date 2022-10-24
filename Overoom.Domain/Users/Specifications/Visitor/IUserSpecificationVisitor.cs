using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Users.Specifications.Visitor;

public interface IUserSpecificationVisitor : ISpecificationVisitor<IUserSpecificationVisitor, User>
{
    void Visit(UserByEmailSpecification specification);
    void Visit(UserByIdSpecification specification);
}