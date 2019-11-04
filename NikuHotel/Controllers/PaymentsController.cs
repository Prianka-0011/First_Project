using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NikuHotel.Data;
using NikuHotel.Models;
using NikuHotel.ViewModel;

namespace NikuHotel.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Payment.Include(p => p.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                
                .Include(p => p.Customer)
                .SingleOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }
        // GET: Payments/AutoCreate
        [HttpGet]
        public IActionResult AutoCreate()
        {

            var date = HttpContext.Session.GetObject<AutoBookings>("date");
            var cartrooms = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
            DateTime date1;
            DateTime date2;
            var totalday = 0.0;


            var amount = 0.0;
            var Totalamount = 0.0;
            date1 = Convert.ToDateTime(date.CheckInTime);
            date2 = Convert.ToDateTime(date.CheckOutTime);
            TimeSpan ts = date2 - date1;
            totalday = ts.TotalDays;
            foreach (var item in cartrooms)
            {
                amount = amount + item.Room.Price;

            }
            Totalamount = totalday * amount;
            ViewBag.totalamount = Totalamount;
            return View();
        }
        // POST: Payments/AutoCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoCreate(FinalPayment finalPayment)
        {
            var customerId = HttpContext.Session.GetInt32("customerId");
            
            if (ModelState.IsValid)
            {
                Payment payment = new Payment()
                {
                    CustomerId = Convert.ToInt32(customerId),
                    PaymentDate = finalPayment.PaymentDate,
                    Amount = finalPayment.Amount,
                    Method = finalPayment.Method

                };
                return RedirectToAction("FinalReport",payment);
            }

            //ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "Address", payment.CustomerId);
            else
            {
                return View();
            }
            
        }
        public IActionResult FinalReport(Payment payment)
        {
            
            var cartrooms = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
            var customerName = HttpContext.Session.GetString("customerName");
            var date = HttpContext.Session.GetObject<AutoBookings>("date");
            if (customerName != null)
            {
                ViewBag.customerName = customerName;
            }
            ViewBag.cartroomsList = cartrooms;
            ViewBag.paymentdate = payment.PaymentDate;
            ViewBag.amount = payment.Amount;
            ViewBag.method = payment.Method;
            ViewBag.checkIn = date.CheckInTime;
            ViewBag.checkOut = date.CheckOutTime;
            


            return View(cartrooms);
        }

        public IActionResult AutoPaymentDetails(Payment payment)
        {
            var customer = _context.Customer.Where(c => c.id == payment.CustomerId).SingleOrDefault();

            ViewBag.customerName = customer.Name;
            return View(payment);
        }
        // GET: Payments/Create
        public IActionResult Create()
        {
            
            ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "Name");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,PaymentDate,Amount,Method,CustomerId")] Payment payment)
        {
            var  cartrooms = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "Address", payment.CustomerId);
            return View(payment);
        }
        //payment create
        [HttpGet]
        public IActionResult PaymentBookig()
        {   
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        

        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> PaymentBookig(PaymentBooking paymentBooking)
        {
            
            var cartrooms = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
            
            
            var customerId = HttpContext.Session.GetInt32("customerId");
            DateTime date1;
            DateTime date2;
            var totalday=0.0;
            var totalprice = 0.0;
            var amount = 0.0;
            if (ModelState.IsValid)
            {
                if (customerId != null)
                {

                    foreach (var item in cartrooms)
                    {
                        var roomId = item.Room.id;
                        Booking bk = new Booking();
                        bk.RoomId = roomId;
                        bk.BookingDate = paymentBooking.BookingDate;
                        bk.CheckInTime = paymentBooking.CheckInTime;
                        bk.CheckOutTime = paymentBooking.CheckOutTime;
                        bk.CustomerId = Convert.ToInt32(customerId);
                        _context.Add(bk);
                        await _context.SaveChangesAsync();
                         date1 = Convert.ToDateTime(bk.CheckInTime);

                         date2 = Convert.ToDateTime(bk.CheckOutTime);

                        TimeSpan ts = date2 - date1; // Assuming date2 will be greater than date1

                        totalday = ts.TotalDays;
                        var price = item.Quantity * item.Room.Price;
                        totalprice = price + totalprice;
                        amount = totalday * totalprice;
                    }
                    
                    

                    Payment payment = new Payment()
                    {
                        PaymentDate = paymentBooking.PaymentDate,
                        Amount = amount,
                        Method = paymentBooking.Method,
                        CustomerId = Convert.ToInt32(customerId)
                        
                    };

                    _context.Add(payment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CustomerRoomView", "Home");
                }
                else
                {
                    return NotFound();
                }
            }

            //ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "Address", payment.CustomerId);
            return View();
        }
        public IActionResult Report()
        {

            return View();
        }
        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }
            
            ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "Address", payment.CustomerId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,PaymentDate,Amount,Method,CustomerId")] Payment payment)
        {
            if (id != payment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.id))
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
            
            ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "Address", payment.CustomerId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                
                .Include(p => p.Customer)
                .SingleOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.id == id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.id == id);
        }
    }
}
