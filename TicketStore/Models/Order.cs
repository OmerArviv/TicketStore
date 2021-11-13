using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Display (Name = "Number of tickets orderd")]
        public int NumOfTickets { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        [Display(Name = "User Name")]
        public string UserName{ get; set; }
        public Event Event { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderTime { get; set; }
        [Display(Name = "Order Total Amount")]
        public int TotalAmount { get; set; }

    }
}
