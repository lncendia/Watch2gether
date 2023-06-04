using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rating.Specifications.Visitor;

public interface IRatingSpecificationVisitor : ISpecificationVisitor<IRatingSpecificationVisitor, Rating>
{
    void Visit(UserRatingsSpecification specification);
}