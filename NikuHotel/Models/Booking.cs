using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.Models
{
    public class Booking
    {
        public int id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } 
        [Required]
        public DateTime CheckInTime { get; set; }
        [Required]
        public DateTime CheckOutTime { get; set; }


        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }

       
    }
}
