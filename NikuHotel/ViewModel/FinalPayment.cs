using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.ViewModel
{
    public class FinalPayment
    {
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string Method { get; set; }
    }
}
