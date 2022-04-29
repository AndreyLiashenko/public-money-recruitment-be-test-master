using VacationRental.Entities.Core;

namespace VacationRental.DAL.Contracts
{
    public interface IRentalRepository
    {
        public Task<int> AddRentalAsync(Rental model);
        public Task<Rental> GetRentalAsync(int Id);
        public int GetLastIdAsync();
    }
}
