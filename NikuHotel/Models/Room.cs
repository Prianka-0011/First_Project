using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.Models
{
    public class Room
    {
        public int id { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Specification { get; set; }
        [Required]
        public double Price { get; set; }

        public List<Booking> Bookings { get; set; }
        public byte[] RoomImage { get; set; }

    }
}
