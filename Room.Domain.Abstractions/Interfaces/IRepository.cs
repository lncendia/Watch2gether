using Room.Domain.Ordering.Abstractions;
using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Abstractions.Interfaces;

public interface IRepository<T, in TK, out TX, out TM> where T : class
    where TX : ISpecificationVisitor<TX, T>
    where TM : ISortingVisitor<TM, T>
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TK id);
    Task<T?> GetAsync(TK id);

    Task<IReadOnlyCollection<T>> FindAsync(ISpecification<T, TX>? specification, IOrderBy<T, TM>? orderBy = null, int? skip = null,
        int? take = null);

    Task<int> CountAsync(ISpecification<T, TX>? specification);
}