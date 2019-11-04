using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NikuHotel.Data;
using NikuHotel.Models;
using NikuHotel.ViewModel;

namespace NikuHotel.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles ="Admin")]
        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Booking.Include(b => b.Customer).Include(b => b.Room);
            return View(await applicationDbContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult AutoBooking()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>AutoBooking(AutoBookings autoBookings )
        {
            var cartrooms = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
            var customerId = HttpContext.Session.GetInt32("customerId");
            HttpContext.Session.SetObject("date", autoBookings);
            List<Booking> book = new List<Booking>();
            
            if (ModelState.IsValid)
            {
                if (customerId != null)
                {

                    foreach (var item in cartrooms)
                    {
                        var roomId = item.Room.id;
                        Booking bk = new Booking();
                        bk.RoomId = roomId;
                        bk.BookingDate = autoBookings.BookingDate;
                        bk.CheckInTime = autoBookings.CheckInTime;
                        bk.CheckOutTime = autoBookings.CheckOutTime;
                        bk.CustomerId = Convert.ToInt32(customerId);
                        
                        _context.Add(bk);
                        await _context.SaveChangesAsync();
                        
                    }

                    
                    return RedirectToAction("Report",autoBookings);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }
        public IActionResult Report(AutoBookings autoBookings)
        {
            DateTime date1;
            DateTime date2;
            var totalday = 0.0;

            var amount = 0.0;
            var Totalamount = 0.0;
            var cartrooms = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
            var customerName = HttpContext.Session.GetString("customerName");
            if (customerName != null)
            {
                ViewBag.customerName = customerName;
            }
            foreach (var item in cartrooms)
            {
                amount = amount + item.Room.Price;

            }
            //foreach (var item in book)
           
                date1 = Convert.ToDateTime(autoBookings.CheckInTime);
                date2 = Convert.ToDateTime(autoBookings.CheckOutTime);
                TimeSpan ts = date2 - date1;
                totalday = ts.TotalDays;
                    
                Totalamount = totalday * amount;
                ViewBag.totalamount = Totalamount;
                return View(cartrooms);
            
        }
        [Authorize(Roles = "Admin")]
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .SingleOrDefaultAsync(m => m.id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        
        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "id", "Address");
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "id", "Category");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,BookingDate,CheckInTime,CheckOutTime,CustomerId,RoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                var room = _context.Room.Where(c => c.id == booking.RoomId).SingleOrDefault();
                var price = room.Price;



                

               return View();
            }

           
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "id", "Address", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "id", "Category", booking.RoomId);
            return View(booking);
        }
        [Authorize(Roles = "Admin")]
        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.SingleOrDefaultAsync(m => m.id == id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "id", "Address", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "id", "Category", booking.RoomId);
            return View(booking);
        }
        [Authorize(Roles = "Admin")]
        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,BookingDate,CheckInTime,CheckOutTime,CustomerId,RoomId")] Booking booking)
        {
            if (id != booking.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "id", "Address", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "id", "Category", booking.RoomId);
            return View(booking);
        }
        [Authorize(Roles = "Admin")]
        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .SingleOrDefaultAsync(m => m.id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        [Authorize(Roles = "Admin")]
        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.SingleOrDefaultAsync(m => m.id == id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.id == id);
        }
    }
}
