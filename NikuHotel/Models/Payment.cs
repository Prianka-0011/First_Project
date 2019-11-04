using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.Models
{
    public class Payment
    {
        public int id { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string Method { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        
    }
}
