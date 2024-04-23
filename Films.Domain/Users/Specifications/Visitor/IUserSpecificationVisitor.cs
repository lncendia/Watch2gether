using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Users.Specifications.Visitor;

public interface IUserSpecificationVisitor : ISpecificationVisitor<IUserSpecificationVisitor, User>
{
    void Visit(UserByIdsSpecification spec);
}