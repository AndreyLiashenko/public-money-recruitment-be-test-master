namespace VacationRental.Entities.DTO.Calendar
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<CalendarUnitViewModel> PreparationTimes { get; set; }
    }
}
