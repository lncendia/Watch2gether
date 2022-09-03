namespace Watch2gether.Domain.Specifications.Abstractions;

public interface ISpecification <in T, in TVisitor>  where TVisitor : ISpecificationVisitor<TVisitor, T>
{
    bool IsSatisfiedBy(T item);
    void Accept (TVisitor visitor);
}