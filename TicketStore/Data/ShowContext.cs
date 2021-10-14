using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketStore.Models;

namespace TicketStore.Data
{
    public class ShowContext: DbContext
    {
        public ShowContext(DbContextOptions<ShowContext> options) : base(options) { }
        public DbSet<Event> Event { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<TicketOrder> TicketOrders { get; set; }
    }
}
