using Microsoft.AspNetCore.Http;
using NikuHotel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NikuHotel.ViewModel
{
    public class ItemofRoom
    {
        public Room Room { get; set; }
        public int Quantity { get; set; }
    }
}
