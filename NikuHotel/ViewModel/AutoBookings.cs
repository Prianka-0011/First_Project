using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.ViewModel
{
    public class AutoBookings
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
        [Required]
        public DateTime CheckInTime { get; set; }
        [Required]
        public DateTime CheckOutTime { get; set; }
    }
}
