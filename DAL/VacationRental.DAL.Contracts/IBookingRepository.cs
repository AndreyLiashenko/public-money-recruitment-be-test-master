using VacationRental.Entities.Core;
using VacationRental.Entities.DTO;

namespace VacationRental.DAL.Contracts
{
    public interface IBookingRepository
    {
        public Task<CreatedSPModel<int>> AddBookingAsync(Booking model);
        public Task<Booking> GetBookingAsync(int Id);

        public IQueryable<Booking> GetCalendarData(int rentalId, DateTime start, int nights);
    }
}
