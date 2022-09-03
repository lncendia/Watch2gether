using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Abstractions.Interfaces;

public interface IRepository<T, in TK, out TX, out TM> where T : class
    where TX : ISpecificationVisitor<TX, T>
    where TM : ISortingVisitor<TM, T>
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IList<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IList<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IList<T> entities);
    Task<T?> GetAsync(TK id);

    Task<IList<T>> FindAsync(ISpecification<T, TX>? specification, IOrderBy<T, TM>? orderBy = null, int? skip = null,
        int? take = null);

    Task<int> CountAsync(ISpecification<T, TX>? specification);
}