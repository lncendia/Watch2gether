using Films.Domain.Specifications.Abstractions;
using Films.Domain.Users.Entities;

namespace Films.Domain.Users.Specifications.Visitor;

public interface IUserSpecificationVisitor : ISpecificationVisitor<IUserSpecificationVisitor, User>
{
    void Visit(UsersByIdsSpecification spec);
}