using AutoMapper;
using VacationRental.Entities.Core;
using VacationRental.Entities.DTO.Rental;

namespace VacationRental.Mapper.RentalProfile
{
    public class RentalMappingProfile : Profile
    {
        public RentalMappingProfile()
        {
            CreateMap<Rental, RentalViewModel>()
                .ForMember(dest => dest.PreparationTimeInDays, o => o.MapFrom(x => x.PreparationTime));

            CreateMap<RentalBindingModel, Rental>()
                 .ForMember(dest => dest.PreparationTime, o => o.MapFrom(x => x.PreparationTimeInDays));
        }
    }
}
