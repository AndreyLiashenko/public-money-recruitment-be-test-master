using System;
namespace VacationRental.Entities.Core
{
    public class Rental
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int? PreparationTime { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
