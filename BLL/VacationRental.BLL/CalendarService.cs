using VacationRental.BLL.Contracts;
using VacationRental.DAL.Contracts;
using VacationRental.Entities.Core;
using VacationRental.Entities.DTO.Calendar;

namespace VacationRental.BLL
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingRepository _bookingRepository;

        public CalendarService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public CalendarViewModel GetCalendarData(int rentalId, DateTime start, int nights)
        {
            var data = _bookingRepository.GetCalendarData(rentalId, start, nights);
            var preparationTime = data.FirstOrDefault().Rental?.PreparationTime ?? 0;

            var dates = MapEachDay(data, preparationTime).ToLookup(x => x.Start)
                .Select(y => new CalendarDateViewModel
                {
                    Date = y.Key.Date,
                    Bookings = y.Where(x => !x.IsPreparationTime).Select(a => new CalendarBookingViewModel
                    {
                        Id = a.Id,
                        Unit = a.Unit
                    }).ToList(),
                    PreparationTimes = y.Where(x => x.IsPreparationTime).Select(a => new CalendarUnitViewModel
                    {
                        Unit = a.Unit
                    }).ToList()
                })
                .ToList();

            if(dates.OrderByDescending(x => x.Date).First().Date < start.AddDays(nights - 1).Date)
            {
                var notBookedDays = (start.AddDays(nights - 1).Date - dates.OrderByDescending(x => x.Date).First().Date).TotalDays;
                for(int i = 1; i <= notBookedDays; i++)
                {
                    var item = new CalendarDateViewModel
                    {
                        Date = dates.OrderByDescending(x => x.Date).First().Date.AddDays(1),
                        Bookings = new List<CalendarBookingViewModel>(),
                        PreparationTimes = new List<CalendarUnitViewModel>()
                    };

                    dates.Add(item);
                }
            }

            return new CalendarViewModel { RentalId = rentalId, Dates = dates.Where(x => x.Date < start.AddDays(nights).Date).ToList() };
        }

        #region private methods
        private List<CalendarBookingDto> MapEachDay(IQueryable<Booking> data, int preparationTime)
        {
            var result = new List<CalendarBookingDto>();

            foreach(Booking booking in data)
            {
                int night = 1;
                for (var day = booking.Start.Date; day.Date <= booking.End.Date; day = day.AddDays(1))
                {
                    var newBooking = new CalendarBookingDto()
                    {
                        Id = booking.Id,
                        RentalId = booking.RentalId,
                        Unit = booking.Unit,
                        Start = day
                    };

                    if(preparationTime != 0)
                    {
                        newBooking.IsPreparationTime = booking.Nights < night;
                    }

                    night++;
                    result.Add(newBooking);
                }
            }

            return result;
        }
        #endregion
    }
}
