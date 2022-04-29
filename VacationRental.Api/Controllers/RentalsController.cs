using Microsoft.AspNetCore.Mvc;
using VacationRental.BLL.Contracts;
using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Rental;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId)
        {
            var result = await _rentalService.GetRentalAsync(rentalId);
            if (result == null) return NotFound("Rental not found");
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post([FromBody] RentalBindingModel model)
        {
            if (model.Units <= 0) return BadRequest("Unit must be positive");

            if (model.PreparationTimeInDays != null && model.PreparationTimeInDays < 0) return BadRequest("PreparationTimeInDays must be positive");

            return await _rentalService.AddRentalAsync(model);
        }
    }
}
