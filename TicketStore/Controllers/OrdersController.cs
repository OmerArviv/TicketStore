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
            var orders = _context.Order.Include(o => o.Costumer).Include(o => o.Event);
            if(orders.FirstOrDefault() == null || orders == null)
            {
                return View("NotFound");
            }
            var joinQuery = from eve in _context.Event
                            join order in _context.Order on eve.Id equals order.EventId into pordGroup
                            from ord2 in pordGroup
                            orderby ord2.TotalAmount descending
                            select ord2;

            if(joinQuery != null)
            {
                ViewData["join"] = "The user with the most expensive purchase is " + joinQuery.FirstOrDefault().Costumer.Email + " with " +
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
                .Include(o => o.Costumer)
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
                    _context.Update(order);
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
                .Include(o => o.Costumer)
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
