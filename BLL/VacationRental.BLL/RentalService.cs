using AutoMapper;
using VacationRental.BLL.Contracts;
using VacationRental.DAL.Contracts;
using VacationRental.Entities.Core;
using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Rental;

namespace VacationRental.BLL
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public RentalService(
            IRentalRepository rentalRepository,
            IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> AddRentalAsync(RentalBindingModel model)
        {
            var rental = _mapper.Map<Rental>(model);
            var result = await _rentalRepository.AddRentalAsync(rental);

            return  new ResourceIdViewModel { Id = result };
        }

        public async Task<RentalViewModel> GetRentalAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalAsync(id);
            return _mapper.Map<RentalViewModel>(rental);
        }
    }
}
