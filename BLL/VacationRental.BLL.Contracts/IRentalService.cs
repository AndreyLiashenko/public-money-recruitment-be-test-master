using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Rental;

namespace VacationRental.BLL.Contracts
{
    public interface IRentalService
    {
        public Task<ResourceIdViewModel> AddRentalAsync(RentalBindingModel model);
        public Task<RentalViewModel> GetRentalAsync(int Id);
    }
}
