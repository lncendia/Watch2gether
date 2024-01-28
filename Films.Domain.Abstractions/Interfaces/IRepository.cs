using Films.Domain.Ordering.Abstractions;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Abstractions.Interfaces;

public interface IRepository<T, in TK, out TX, out TM> where T : class
    where TX : ISpecificationVisitor<TX, T>
    where TM : ISortingVisitor<TM, T>
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TK id);
    Task<T?> GetAsync(TK id);

    Task<IList<T>> FindAsync(ISpecification<T, TX>? specification, IOrderBy<T, TM>? orderBy = null, int? skip = null,
        int? take = null);

    Task<int> CountAsync(ISpecification<T, TX>? specification);
}