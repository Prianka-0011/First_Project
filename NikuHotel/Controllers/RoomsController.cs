using System;
using System.Collections.Generic;
using System.IO;
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
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult allsessionroom()
        {

            return View();
        }

        public IActionResult AddToCartRooms(int id)
        {
            bool itemfound = false;
            List<ItemofRoom> cartroom = new List<ItemofRoom>();
            //var room = _context.Room.Find(id);
            var room = _context.Room.FirstOrDefault(r => r.id == id);
            var totalqnty = 0;
            cartroom = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
            if (room != null)
            {
                if (cartroom != null)
                {
                    List<ItemofRoom> newcart = new List<ItemofRoom>();
                    foreach (var item in cartroom)
                    {
                        if (item.Room.id == room.id)
                        {
                            
                            itemfound = true;
                            item.Quantity = item.Quantity + 1;
                        }
                       
                        newcart.Add(item);
                    }
                    if (itemfound == false)
                    {
                        newcart.Add(new ItemofRoom()
                        {
                            Room = room,
                            Quantity = 1,
                           
                        });
                        
                    }
                    HttpContext.Session.SetObject("cart", newcart);
                    foreach (var item in newcart)
                    {
                        totalqnty = item.Quantity + totalqnty;
                    }
                    HttpContext.Session.SetInt32("qnty", totalqnty);
                    ViewBag.qunty = totalqnty;
                    return View(newcart);
                }
                else
                {
                    List<ItemofRoom> newcart = new List<ItemofRoom>();
                   
                   
                    newcart.Add(new ItemofRoom()
                    {
                        Room = room,
                        Quantity = 1
                    });
                   
                    HttpContext.Session.SetObject("cart", newcart);
                    return View(newcart);
                }
                
            }
            else
            {
                return View(cartroom);
            }

        }

        public IActionResult ShowCartRooms()
        {

            return View();  
        }
        public IActionResult RemoveFromCart(int id)
        {
            if (id > 0)
            {
                List<ItemofRoom> cartproducts = new List<ItemofRoom>();
                cartproducts = HttpContext.Session.GetObject<List<ItemofRoom>>("cart");
                if (cartproducts != null)
                {
                    List<ItemofRoom> newcart = new List<ItemofRoom>();
                    foreach (var item in cartproducts)
                    {
                        if (item.Room.id != id)
                        {
                            newcart.Add(item);
                        }
                    }
                    HttpContext.Session.SetObject("cart", newcart);
                }
            }
            return RedirectToAction("AddToCartRooms");
        }

        // GET: Rooms
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Room.ToListAsync());
        }

        // GET: Rooms/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .SingleOrDefaultAsync(m => m.id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
        [Authorize(Roles = "Admin")]
        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomView roomView)
        {
            if (ModelState.IsValid)
            {
                Room r = new Room();
                r.Category = roomView.Category;
                r.Number = roomView.Number;
                
                r.Price = roomView.Price;
                r.Specification = roomView.Specification;
                using (var ms = new MemoryStream())
                {
                    await roomView.RoomImage.CopyToAsync(ms);
                    r.RoomImage = ms.ToArray();
                }
                _context.Add(r);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomView);
        }
        [Authorize(Roles = "Admin")]
        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.SingleOrDefaultAsync(m => m.id == id);
            RoomView roomView = new RoomView();
           
            if (room == null)
            {
                return NotFound();
            }
            roomView.id = room.id;
            roomView.Category = room.Category;
            roomView.Number = room.Number;
            roomView.Specification = room.Specification;
            roomView.Price = room.Price;
            return View(roomView);
        }
        [Authorize(Roles = "Admin")]
        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Category,Number,Specification,Price,RoomImage")] RoomView roomView)
        {
            if (id != roomView.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Room room = new Room();
                room.id = roomView.id;
                room.Category = roomView.Category;
                room.Number = roomView.Number;
                room.Specification = roomView.Specification;
                room.Price = roomView.Price;
                using (var ms = new MemoryStream())
                {
                    await roomView.RoomImage.CopyToAsync(ms);
                    room.RoomImage = ms.ToArray();
                }

                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.id))
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
            return View(roomView);
        }
        [Authorize(Roles = "Admin")]
        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .SingleOrDefaultAsync(m => m.id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.SingleOrDefaultAsync(m => m.id == id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.id == id);
        }
    }
}
