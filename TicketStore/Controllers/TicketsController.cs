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

        // GET: Tickets

        //public async Task<IActionResult> Buy(HashMap<string, int> hashMap, int count, int eventID)
        //{

        //    var id = User.Claims.First(c => c.Type == "UserId")?.Value;
        //    var user = from u in _context.User where
        //               u.Id.ToString().Equals(id) select u;

        //    var e = from t in _context.Event where t.Id == eventID select t;

            

        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            
            var tmp = User.Claims.First(c => c.Type == "UserId").Value;
            if(tmp != null)
            {
                var id = int.Parse(tmp);
                //var tempUser = _context.User.FirstOrDefault();
                var tempUser = from t in _context.User where id == t.Id select t;
                var tickets = new List<Ticket>();

                foreach (Ticket t in _context.Tickets)
                {

                    if (t.UserID == id)
                    {
                        var events = _context.Event.FirstOrDefault(e => e.Id == t.EventID);
                        t.Event = events;
                        tickets.Add(t);
                    }

                }
                if (tempUser == null)
                    return RedirectToAction("Index", "Event");
                else
                {
                    return View(tickets);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
           
            
            //var events = from e in _context.Event select e;
            //ViewData["events"] = events;


            //if (_context.Tickets.Any())
            //{
            //    return View(await _context.Tickets.ToListAsync());
            //}
            //return View();
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var ticket = await _context.Tickets
                .Include(c=>c.Event)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return View("NotFound");
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
        public async Task<IActionResult> SortByGenre(string genre)
        {
            var tmp = User.Claims.First(c => c.Type == "UserId").Value;
            if (tmp != null)
            {
                var id = int.Parse(tmp);
                //var tempUser = _context.User.FirstOrDefault();
                var tempUser = from t in _context.User where id == t.Id select t;
                var tickets = new List<Ticket>();

                foreach (Ticket t in _context.Tickets)
                {

                    if (t.UserID == id)
                    {
                        var events = _context.Event.FirstOrDefault(e => e.Id == t.EventID);
                        t.Event = events;
                        tickets.Add(t);
                    }

                }
                var endTickets = new List<Ticket>();
                var ticketsByGenre = tickets.GroupBy(g => g.Event.Genre);
                foreach(var group in ticketsByGenre)
                {
                    if (group.Key.ToString().Equals(genre))
                    {
                        foreach (var ti in group)
                        {
                            endTickets.Add(ti);
                        }
                    }
                }
       
                if (tempUser == null)
                    return RedirectToAction("Index", "Event");
                else
                {
                    return View(endTickets);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Available,Price,Seat,EventID,UserID")] Ticket ticket)
        {
            if(ticket==null) { return View("NotFount"); }
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
                return View("NotFound");
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return View("NotFound");
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
                return View("NotFound");
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
                        return View("NotFound");
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
                return View("NotFound");
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return View("NotFound");
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
