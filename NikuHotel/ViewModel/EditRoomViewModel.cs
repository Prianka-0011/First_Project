using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.ViewModel
{
    public class EditRoomViewModel
    {
        [Required]
        public string Category { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Specification { get; set; }
        [Required]
        public double Price { get; set; }

        public IFormFile RoomImage { get; set; }
    }
}
