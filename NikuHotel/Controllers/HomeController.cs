using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikuHotel.Data;
using NikuHotel.Models;

namespace NikuHotel.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Customer")]
        public IActionResult CustomerRoomView(string srctxt)
        {
            var ob = HttpContext.Session.GetInt32("qnty");
            ViewBag.ob = ob;
            IQueryable<Room> rooms = _context.Room;
            ViewBag.srctxt = srctxt;
            if (!string.IsNullOrEmpty(srctxt))
            {
                srctxt = srctxt.ToLower();
                rooms = rooms.Where(r => r.Category.ToLower().Contains(srctxt));
            }
            ViewBag.count = rooms.Count();
            
            //ViewData["Message"] = "Your contact page.";
            //var room = _context.Room.ToList();
            //List<Room> room1 = new List<Room>();
            //var bookingns = _context.Booking.ToList();
            //bool flag = false;
            //foreach (var item in room)
            //{
            //    foreach (var item2 in bookingns)
            //    {
            //        if(item.id==item2.RoomId)
            //        {
            //            flag = true;
            //            //room=item
            //        }
            //    }
            //    if(flag==false)
            //    {
            //        room1.Add(item);
            //    }
            //}
            return View(rooms);
        }
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.SingleOrDefaultAsync(m => m.id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
