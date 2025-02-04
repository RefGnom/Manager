namespace Data.Repositories;

public interface IRepository<T>
    where T : class
{
    Task CreateAsync(T timer);
    Task<T> FindAsync(Guid id);
    Task SaveAsync();
    Task DeleteAsync(Guid id);
}