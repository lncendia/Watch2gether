namespace Room.Domain.Specifications.Abstractions;

public interface ISpecificationVisitor<TVisitor, T>  where TVisitor : ISpecificationVisitor<TVisitor, T>
{
    void Visit(AndSpecification<T, TVisitor> spec);
    void Visit(OrSpecification<T, TVisitor> spec);
    void Visit(NotSpecification<T, TVisitor> spec);
}