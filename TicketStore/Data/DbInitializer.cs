using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketStore.Models;

namespace TicketStore.Data
{
    public class DbInitializer
    {
        public static void Initialize(ShowContext context) {
            context.Database.EnsureCreated();

            if(context.User.Any()) { return; }
            var users = new Models.User[]
            {
                new Models.User{Birthdate=DateTime.Parse("2021-05-17"), CartId=0, Email="rrttou@gmail.com", FirstName="Raz", Gender=0, LastName="Yaniv", Password="1234", PasswordConfirm="1234", UserName="rrttou", Type=0 }


            };
            context.SaveChanges();

            if (context.Event.Any()) { return; }

            var events = new Models.Event[] {
                new Models.Event{ArtistName="Omer Adam", AvailableTickets=1000,  Date=DateTime.Parse("2021-11-11"), Genre="Music", Place="Qeusarya", Description="Last Omer Adam show of 2021!"},
                new Event{ArtistName="Eyal Golan", AvailableTickets=2500,  Date=DateTime.Parse("2021-12-12"), Genre="Music", Place="Qeusarya", Description="Eyal Golan comes up with new singles" },
                new Event{ArtistName="Maccabi Haifa", AvailableTickets=30000,  Date=DateTime.Parse("2021-12-01"), Genre="Sport", Place="Sammy-Offer stadium, Haifa", Description="Maccabi Haifa vs Maccabi TLV"},
                new Event{ArtistName="Pixies", AvailableTickets=1400,  Date=DateTime.Parse("2022-01-01"), Genre="Music", Place="Barbi, TLV", Description="The Greatest rock band with first new year concert!"},
                new Event{ArtistName="Sarit Hadad", AvailableTickets=1700, Date=DateTime.Parse("2021-10-16"), Genre="Music", Place="Rishon Lezion", Description="The Middle East queen with a brand new show"}
            };

            foreach (Event e in events)
            {
                context.Event.Add(e);
            }


            context.SaveChanges();

            if (context.Tickets.Any()) { return; }

            var tickets = new Models.Ticket[] {
                new Models.Ticket{Description="Omer Adam show", Price=200, Available=true, EventID=events[0].Id},
                new Ticket{Description="Eyal Golan show", Price=180, Available=true, EventID=events[1].Id},
                new Ticket{Description="Maccabi Haifa vs Maccabi TLV", Price=80, Available=true, EventID=events[2].Id},
                new Ticket{Description="Pixies show", Price=100, Available=true, EventID=events[3].Id},
                new Ticket{Description="Sarit Hadad show", Price=100, Available=true, EventID=events[4].Id}
            };

            foreach (Ticket t in tickets)
            {
                context.Tickets.Add(t);
            }

            context.SaveChanges();

        }

    }
}
