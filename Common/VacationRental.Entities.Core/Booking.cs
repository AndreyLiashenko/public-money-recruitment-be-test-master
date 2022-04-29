namespace VacationRental.Entities.Core
{
    public class Booking
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int Nights { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Unit { get; set; }
        public Rental Rental { get; set; }
    }
}
