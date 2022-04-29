using Microsoft.AspNetCore.Mvc;
using VacationRental.BLL.Contracts;
using VacationRental.Entities.DTO.Calendar;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly IRentalService _rentalService;

        public CalendarController(
            ICalendarService calendarService,
            IRentalService rentalService)
        {
            _calendarService = calendarService;
            _rentalService = rentalService;
        }

        [HttpGet]
        public async Task<ActionResult<CalendarViewModel>> Get(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0) return BadRequest("Nigts must be positive and more than 0");

            var result = await _rentalService.GetRentalAsync(rentalId);
            if (result == null) return NotFound("Rental not found");

            return _calendarService.GetCalendarData(rentalId, start, nights);
        }
    }
}
