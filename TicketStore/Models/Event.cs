using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace TicketStore.Models
{
    public class Event
    {
        public enum Genre1
        {
            Movie,
            Sport,
            Music
        }

        public Genre1 Genre2 { get; set; }

        public int Id { get; set; }

        public string ArtistName { get; set; }

        public string Place { get; set; }

        public string ImageUrl { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public int stars { get; set; }
        public int MinPrice { get; set; }

        [Display(Name= "Check available tickets")]
        public int AvailableTickets { get; set; }

        public string Genre { get; set; }

        
        public string Description { get; set; }
        [Display(Name = "Show Time")]
        public DateTime Date { get; set; }
        

        public ICollection<Ticket> Tickets{ get; set; }

        public double LocationX { get; set; }
        public double LocationY { get; set; }





    }
}
