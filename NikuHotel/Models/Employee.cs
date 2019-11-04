using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.Models
{
    public class Employee
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [MaxLength(15),Required]
        public string ContactNo { get; set; }
        [Required]
        public string  NidOrPassport { get; set; }

    }
}
