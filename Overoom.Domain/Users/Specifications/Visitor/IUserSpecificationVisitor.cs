using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Entities;

namespace Overoom.Domain.Users.Specifications.Visitor;

public interface IUserSpecificationVisitor : ISpecificationVisitor<IUserSpecificationVisitor, User>
{
    void Visit(UserByEmailSpecification specification);
    void Visit(UserByIdSpecification specification);
    void Visit(UserByIdsSpecification specification);
    void Visit(UserByNameSpecification specification);
}