using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Specifications;

public class OrSpecification<T, TVisitor>(ISpecification<T, TVisitor> left, ISpecification<T, TVisitor> right)
    : ISpecification<T, TVisitor>
    where TVisitor : ISpecificationVisitor<TVisitor, T>
{
    public ISpecification<T, TVisitor> Left { get; } = left;
    public ISpecification<T, TVisitor> Right { get; } = right;

    public void Accept(TVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(T obj) => Left.IsSatisfiedBy(obj) || Right.IsSatisfiedBy(obj);
}