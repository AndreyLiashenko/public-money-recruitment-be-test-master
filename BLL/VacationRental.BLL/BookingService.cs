using AutoMapper;
using VacationRental.BLL.Contracts;
using VacationRental.DAL.Contracts;
using VacationRental.Entities.Core;
using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Booking;

namespace VacationRental.BLL
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepository, 
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<CreatedSPModel<int>> AddBookingAsync(BookingBindingModel model)
        {
            var booking = _mapper.Map<Booking>(model);
            var result = await _bookingRepository.AddBookingAsync(booking);

            return result;
        }

        public async Task<BookingViewModel> GetBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingAsync(id);
            return _mapper.Map<BookingViewModel>(booking);
        }
    }
}
