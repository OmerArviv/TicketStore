using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketStore.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }
        public ICollection<TicketOrder> TicketOrders { get; set; } 
        public User User { get; set; }
        public bool IsInCart { get; set; }//the database will update only when user confirm and make the order
                                          //

    }
}
