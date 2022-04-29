using Microsoft.EntityFrameworkCore;

namespace VacationRental.DAL.Core
{
    public class BaseRepository<T, TId> : IRepository<T, TId>,
                                                   IAsyncRepository<T, TId> where T : class
    {
        #region Properties
        protected readonly VacationRentalContext _dbContext;
        #endregion

        #region Constructors

        public BaseRepository(VacationRentalContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region public methods
        public T GetById(TId id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> ListAll()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public IQueryable<T> QueryableAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
