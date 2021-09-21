using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketStore.Data;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    public class EventsController : Controller
    {
        private readonly ShowContext _context;

        public EventsController(ShowContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            var events = from e in _context.Event select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(myEvent => myEvent.ArtistName.Contains(searchString));
                
            }

            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(s => s.ArtistName);
                    break;
                case "Date":
                    events = events.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(s => s.Date);
                    break;
                default:
                    events = events.OrderBy(s => s.Genre);
                    break;
            }
            

            return View(await events.ToListAsync());


        }


        public async Task<IActionResult> Filter(string filterByGenre)
        {
            var events = from e in _context.Event select e;
            var selectList = new SelectList(
            new List<SelectListItem>
                {
                    new SelectListItem {Text = "Sport", Value = "Sport"},
                    new SelectListItem {Text = "Music", Value = "Music"},
                    new SelectListItem {Text = "Movie", Value = "Movie"},
                }, "Value", "Text");
            SelectList list = selectList;
            ViewData["List"] = list;
            
            switch (filterByGenre)
            {
                case "Sport":
                    events = from k in _context.Event
                             where k.Genre.Equals("Sport")
                             select k;
                    break;
                case "Music":
                    events = from k in _context.Event
                             where k.Genre.Equals("Music")
                             select k;
                    break;
                case "Movie":
                    events = from k in _context.Event
                             where k.Genre.Equals("Movie")
                             select k;
                    break;
                default:
                    events = events.OrderBy(s => s.Genre);
                    break;

            }
            return View(await events.ToListAsync());


        }
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var event1 = await _context.Event.Include(t=>t.Tickets)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (event1 == null)
            {
                return NotFound();
            }

            return View( event1);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public string Index(Event @event)
        {
            return null;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistName,Place,AvailableTickets,Genre,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistName,Place,AvailableTickets,Genre,Date")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }
    }
}
