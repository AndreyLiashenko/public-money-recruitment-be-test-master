using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Mapper.BookingProfile;
using VacationRental.Mapper.RentalProfile;

namespace VacationRental.Mapper
{
    public static class DIHelper
    {
        public static void RegisterMapperServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RentalMappingProfile));
            services.AddAutoMapper(typeof(BookingMappingProfile));
        }
    }
}
