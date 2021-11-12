using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int NumOfTickets { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public User Costumer { get; set; }
        public Event Event { get; set; }
        public DateTime OrderTime { get; set; }

    }
}
