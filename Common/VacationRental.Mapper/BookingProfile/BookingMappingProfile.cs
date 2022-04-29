using AutoMapper;
using VacationRental.Entities.Core;
using VacationRental.Entities.DTO.Booking;

namespace VacationRental.Mapper.BookingProfile
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            CreateMap<Booking, BookingViewModel>();

            CreateMap<BookingBindingModel, Booking>()
                 .ForMember(dest => dest.Start, o => o.MapFrom(x => x.Start.ToString("yyyy-MM-dd")))
                 .ForMember(dest => dest.End, o => o.MapFrom(x => x.Start.AddDays(x.Nights - 1).ToString("yyyy-MM-dd")));
        }
    }
}
