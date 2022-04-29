using VacationRental.Entities.DTO.Calendar;

namespace VacationRental.BLL.Contracts
{
    public interface ICalendarService
    {
        CalendarViewModel GetCalendarData(int rentalId, DateTime start, int nights);
    }
}
