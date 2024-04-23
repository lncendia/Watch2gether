using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Specifications;

public class NotSpecification<T, TVisitor>(ISpecification<T, TVisitor> specification) : ISpecification<T, TVisitor>
    where TVisitor : ISpecificationVisitor<TVisitor, T>
{
    public ISpecification<T, TVisitor> Specification { get; } = specification;

    public void Accept(TVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(T obj) => !Specification.IsSatisfiedBy(obj);
}