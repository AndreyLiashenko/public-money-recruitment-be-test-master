namespace VacationRental.DAL.Core
{
    public interface IAsyncRepository<T, TId>
    {
        Task<T> GetByIdAsync(TId id);
        Task<List<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
