using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketStore.Models
{
    public class TicketOrder
    {
        public int Id { get; set; }
        public Ticket Ticket { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Amount { get; set; }
    }
}
