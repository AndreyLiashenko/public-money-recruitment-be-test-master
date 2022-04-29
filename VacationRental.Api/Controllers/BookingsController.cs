using Microsoft.AspNetCore.Mvc;
using System.Net;
using VacationRental.BLL.Contracts;
using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Booking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            var result = await _bookingService.GetBookingAsync(bookingId);
            if(result == null) return NotFound("Booking not found");
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post([FromBody] BookingBindingModel model)
        {
            if (model.Nights <= 0) return BadRequest("Nigts must be positive and more than 0");

            var result = await _bookingService.AddBookingAsync(model);

            if(result.StatusCode == (int)HttpStatusCode.NotFound) return NotFound(result.Message);

            if (result.StatusCode == (int)HttpStatusCode.BadRequest) return BadRequest(result.Message);

            return Ok(new ResourceIdViewModel { Id = result.CreatedId });
        }
    }
}
