using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Booking;

namespace VacationRental.BLL.Contracts
{
    public interface IBookingService
    {
        public Task<CreatedSPModel<int>> AddBookingAsync(BookingBindingModel model);
        public Task<BookingViewModel> GetBookingAsync(int Id);
    }
}
