using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.User.Specifications.Visitor;

public interface IUserSpecificationVisitor : ISpecificationVisitor<IUserSpecificationVisitor, User.Entities.User>
{
    void Visit(UserByEmailSpecification specification);
    void Visit(UserByIdSpecification specification);
}