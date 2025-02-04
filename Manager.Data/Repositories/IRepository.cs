namespace Data.Repositories;

public interface IRepository<T>
    where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> TryGetByIdAsync(Guid id);
    Task CreateAsync(T item);
    Task UpdateAsync(Guid id, T item);
    Task DeleteAsync(Guid id);
    Task SaveAsync();
}