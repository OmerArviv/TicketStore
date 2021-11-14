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
    public class OrdersController : Controller
    {
        private readonly ShowContext _context;

        public OrdersController(ShowContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            var orders = _context.Order.Include(o => o.Event);
            if(orders.FirstOrDefault() == null || orders == null)
            {
                return View("NotFound");
            }
            var joinQuery = from eve in _context.Event
                            join order in _context.Order on eve.Id equals order.EventId into pordGroup
                            from ord2 in pordGroup
                            orderby ord2.TotalAmount descending
                            select ord2;

            var us = _context.User.Where(a => a.Id == joinQuery.FirstOrDefault().UserId);

            if(joinQuery != null && us.FirstOrDefault() != null)
            {
                ViewData["join"] = "The user with the most expensive purchase is " + us.FirstOrDefault().Email + " with " +
                        joinQuery.FirstOrDefault().TotalAmount + "$";
            }
            

            return View(await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (id == null)
            {
                return View("NotFound");
            }

            var order = await _context.Order
                .Include(o => o.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return View("NotFound");
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumOfTickets,EventId,UserId,OrderTime,TotalAmount")] Order order)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", order.UserId);
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", order.EventId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (id == null)
            {
                return View("NotFound");
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return View("NotFound");
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", order.UserId);
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", order.EventId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumOfTickets,EventId,UserId,OrderTime,TotalAmount")] Order order)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (id != order.Id)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.NumOfTickets <= 0)
                    {
                        return View("NotFound");
                    }
                    Order o = _context.Order.Where(o => o.Id == order.Id).FirstOrDefault();
                    if (order.UserId != o.UserId)
                    {
                        for (int i = 0; i < o.NumOfTickets; i++)
                        {
                            Ticket tempTicket = _context.Tickets.Where(t => (t.UserID == o.UserId) && (t.EventID == o.EventId)).FirstOrDefault();
                            tempTicket.UserID = order.UserId;
                            _context.SaveChanges();
                        }
                        o.UserId = order.UserId;
                        _context.SaveChanges();
                    }
                    if (order.EventId != o.EventId)
                    {
                        for (int i = 0; i < o.NumOfTickets; i++)
                        {
                            Ticket tempTicket = _context.Tickets.Where(t => (t.UserID == o.UserId) && (t.EventID == o.EventId)).FirstOrDefault();
                            tempTicket.EventID = order.EventId;
                            _context.SaveChanges();
                        }
                        o.EventId = order.EventId;
                        _context.SaveChanges();
                    }
                    o.TotalAmount = _context.Event.Where(e => e.Id == order.EventId).FirstOrDefault().MinPrice * order.NumOfTickets;
                    Event e = _context.Event.Where(s => s.Id == order.EventId).FirstOrDefault();
                    int temp = _context.Order.Where(s => s.Id == id).FirstOrDefault().NumOfTickets;
                    int change = 0;
                    Event tempEvent = _context.Event.Where(e => e.Id == order.Id).FirstOrDefault();
                    if (order.NumOfTickets > temp)
                    {
                        change = order.NumOfTickets - temp;
                        if (change > tempEvent.AvailableTickets)
                            return View("NotFound");
                        for (int i = 0; i < change; i++)
                        {
                            Ticket ticket = new Ticket { Description = e.Description, Price = e.MinPrice, Available = false, EventID = order.EventId, Event = _context.Event.FirstOrDefault(e => e.Id == order.EventId), UserID = order.UserId };

                            tempEvent.AvailableTickets--;
                            _context.Tickets.Add(ticket);
                        }
                    }
                    else if (temp > order.NumOfTickets)
                    {
                        change = temp - order.NumOfTickets;
                        for (int i = 0; i < change; i++)
                        {
                            Ticket tempTicket = _context.Tickets.Where(t => (t.UserID == order.UserId) && (t.EventID == order.EventId)).FirstOrDefault();
                            _context.Tickets.Remove(tempTicket);
                            tempEvent.AvailableTickets++;
                            _context.SaveChanges();
                        }
                    }
                    o.NumOfTickets = order.NumOfTickets;
                    _context.SaveChanges();
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", order.UserId);
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", order.EventId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (id == null)
            {
                return View("NotFound");
            }

            var order = await _context.Order
                .Include(o => o.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return View("NotFound");
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Order tempOrder = _context.Order.Where(o => o.Id == id).FirstOrDefault();
            for (int i = 0; i < tempOrder.NumOfTickets; i++)
            {
                Ticket tempTicket = _context.Tickets.Where(t => (t.UserID == tempOrder.UserId) && (t.EventID == tempOrder.EventId)).FirstOrDefault();
                _context.Tickets.Remove(tempTicket);
                _context.SaveChanges();
            }
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
