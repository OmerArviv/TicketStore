 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketStore.Data;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ShowContext _context;

        public TicketsController(ShowContext context)
        {
            _context = context;
        }

        //tests for dibug
        //public async Task<IActionResult> test1()
        //{
        //    var events = new Models.Event[] {
        //        new Models.Event{ArtistName="Omer Adam", AvailableTickets=1000,  Date=DateTime.Parse("2021-11-11"), Genre="Music", Place="Qeusarya", Description="Last Omer Adam show of 2021!", Id=1},
        //        new Event{ArtistName="Eyal Golan", AvailableTickets=2500,  Date=DateTime.Parse("2021-12-12"), Genre="Music", Place="Qeusarya", Description="Eyal Golan comes up with new singles", Id=2 },
        //        new Event{ArtistName="Maccabi Haifa", AvailableTickets=30000,  Date=DateTime.Parse("2021-12-01"), Genre="Sport", Place="Sammy-Offer stadium, Haifa", Description="Maccabi Haifa vs Maccabi TLV", Id=3},
        //        new Event{ArtistName="Pixies", AvailableTickets=1400,  Date=DateTime.Parse("2022-01-01"), Genre="Music", Place="Barbi, TLV", Description="The Greatest rock band with first new year concert!", Id=4},
        //        new Event{ArtistName="Sarit Hadad", AvailableTickets=1700, Date=DateTime.Parse("2021-10-16"), Genre="Music", Place="Rishon Lezion", Description="The Middle East queen with a brand new show", Id=5}
        //    };

        //    var tickets = new Models.Ticket[] {
        //        new Models.Ticket{Description="Omer Adam show", Price=200, Available=true, EventID=1},
        //        new Ticket{Description="Eyal Golan show", Price=180, Available=true, EventID=2},
        //        new Ticket{Description="Maccabi Haifa vs Maccabi TLV", Price=80, Available=true, EventID=3},
        //        new Ticket{Description="Pixies show", Price=100, Available=true, EventID=4},
        //        new Ticket{Description="Sarit Hadad show", Price=100, Available=true, EventID=5}
        //    };

        //    return View(_context.Event.ToListAsync());
        //}


        // GET: Tickets

        public async Task<IActionResult> Buy(HashMap<string, int> hashMap, int count, int eventID)
        {

            var id = User.Claims.First(c => c.Type == "UserId")?.Value;
            var user = from u in _context.User where
                       u.Id.ToString().Equals(id) select u;

            var e = from t in _context.Event where t.Id == eventID select t;

            

            return View();
        }
        public async Task<IActionResult> Index()
        {
            
            var events = from e in _context.Event select e;
            ViewData["events"] = events;
            
            
            if (_context.Tickets.Any())
            {
                return View(await _context.Tickets.ToListAsync());
            }
            return View();
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(c=>c.Event)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            // SelectList tmp = new(_context.Event, nameof(Event.Id), nameof(Event.ArtistName));
            var tmp = from e in _context.Event select e;
            ViewData["events"] = tmp;
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Available,Price,Seat,EventID,UserID")] Ticket ticket)
        {
            ticket.Event = _context.Event.Find(ticket.Id);
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SerialNumber,Available,Price,Seat")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
