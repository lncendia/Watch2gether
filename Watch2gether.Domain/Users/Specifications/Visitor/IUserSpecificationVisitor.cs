using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Users.Specifications.Visitor;

public interface IUserSpecificationVisitor : ISpecificationVisitor<IUserSpecificationVisitor, User>
{
    void Visit(UserByEmailSpecification specification);
    void Visit(UserByIdSpecification specification);
}