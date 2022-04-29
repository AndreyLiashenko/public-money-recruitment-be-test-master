using Microsoft.EntityFrameworkCore;
using VacationRental.BLL;
using VacationRental.BLL.Contracts;
using VacationRental.DAL;
using VacationRental.DAL.Contracts;
using VacationRental.DAL.Core;

namespace VacationRental.Api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDiDal(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("VacationRentalDatabase");
            return services
                .AddDbContext<VacationRentalContext>(options => options.UseSqlServer(connectionString))

                .AddTransient<IRentalRepository, RentalRepository>()
                .AddTransient<IBookingRepository, BookingRepository>()
            ;
        }

        public static IServiceCollection AddDiBll(this IServiceCollection services)
        {
            return services
                .AddTransient<IRentalService, RentalService>()
                .AddTransient<IBookingService, BookingService>()
                .AddTransient<ICalendarService, CalendarService>()
            ;
        }
    }
}
