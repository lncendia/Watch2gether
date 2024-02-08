namespace Room.Domain.Abstractions.Interfaces;

public interface IRepository<T, in TK> where T : class
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TK id);
    Task<T?> GetAsync(TK id);
}