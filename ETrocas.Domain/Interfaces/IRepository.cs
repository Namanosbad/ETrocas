namespace ETrocas.Domain.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<T> UpdateAsync(T Tentity);
    Task<bool>ExistAsync(Guid id);
    Task DeleteAsync(Guid id);
}