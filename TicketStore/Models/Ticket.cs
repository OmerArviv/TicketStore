using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketStore.Models
{
    public class Ticket
    {
        //Ticket(int eventid, int userid)
        //{
        //    this.EventID = eventid;
        //    this.UserID = userid;
        //}

        public int Id { get; set; } //Primary key 
        public string Description { get; set; }
        [Required]
        public int EventID{ get; set; }
        public int? UserID { get; set; }


        public bool Available { get; set; }

        public int Price { get; set; }

        public string Seat{ get; set; }

        public Event Event { get; set; }
        public User Costumer { get; set; }
    }
}
