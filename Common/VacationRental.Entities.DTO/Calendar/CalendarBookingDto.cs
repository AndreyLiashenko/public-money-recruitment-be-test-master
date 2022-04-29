using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Entities.DTO.Calendar
{
    public class CalendarBookingDto
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Unit { get; set; }
        public bool IsPreparationTime { get; set; }
    }
}
