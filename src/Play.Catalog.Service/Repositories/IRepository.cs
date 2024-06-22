using Play.Catalog.Service.Emtties;

namespace Play.Catalog.Service.Repositories;

public interface IRepository<T> where T : Entity
{
    Task CreateAsync(T entity);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<T> GetAsync(Guid id);
    Task UpdateAsync(T entity);
    Task RemoveAsync(Guid id);
}
