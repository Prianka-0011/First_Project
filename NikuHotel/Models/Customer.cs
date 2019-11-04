using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.Models
{
    public class Customer
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,MaxLength(15)]
        public string ContactNo { get; set; }
        [Required]
        public string NidOrPassport { get; set; }

        public List<Booking> Bookings { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
