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
            return View(await _context.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(u => u.User)
                .Include(To => To.TicketOrders).ThenInclude(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            int customerId = Models.User.UserConnectedByID.Peek();
            var customer = _context.User.Include(o => o.Orders)
                .ThenInclude(to => to.TicketOrders).ThenInclude(t => t.Ticket)
                .Where(c => c.Id == customerId).FirstOrDefault();
            if (customer != null)
            {
                var order = customer.Orders.Where(o => o.IsInCart).FirstOrDefault();
                if (order != null)
                {
                    order.User = _context.User.Find(customerId);
                    order.IsInCart = false;//the order already confirmed.
                    order.TotalPrice = 0;
                    order.OrderDate = DateTime.Now;
                    foreach (var tic in order.TicketOrders)
                    {
                        order.TotalPrice += tic.Ticket.Price;
                    }
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                    foreach (var to in order.TicketOrders)
                    {
                        to.Order = order;
                    }
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                    _context.User.Update(order.User);
                    await _context.SaveChangesAsync();
                    foreach (var to in order.TicketOrders)
                        _context.Tickets.Find(to.Id).ticketOrders.Add(to);
                    await _context.SaveChangesAsync();
                    foreach (var to in order.TicketOrders)
                    {
                        if ((_context.TicketOrders.Find(to.Id, to.OrderId) == null))
                            _context.TicketOrders.Add(to);
                        else
                            _context.TicketOrders.Update(to);
                        _context.SaveChanges();
                    }
                    ViewBag.orderId = order.OrderID;
                    return View("PaymentApproval");
                }
                else
                {
                    return View("~/Views/Shared/Customers/EmptyShopCart.cshtml");
                }
            }
            return View("PaymentApproval", new Order() { IsInCart = false });
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,OrderDate,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderDate,TotalPrice")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
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
                    if (!OrderExists(order.OrderID))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
