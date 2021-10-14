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
                new Models.User{Birthdate=DateTime.Parse("2021-05-17"), Email="rrttou@gmail.com", FirstName="Raz", Gender=0, LastName="Yaniv", Password="Admin@123", PasswordConfirm="Admin@123", UserName="rrttou", Type=0, IsAdmin=true }


            };
            context.SaveChanges();

            if (context.Event.Any()) { return; }

            var events = new Models.Event[] {
                new Models.Event{ArtistName="Omer Adam", AvailableTickets=1000,  Date=DateTime.Parse("2021-11-11"), Genre="Music", Place="Qeusarya", Description="Last Omer Adam show of 2021!",Urlimage="https://www.israelhayom.co.il/sites/default/files/styles/566x349/public/images/articles/2019/12/23/15770856936271_b.jpg"},
                new Event{ArtistName="Eyal Golan", AvailableTickets=2500,  Date=DateTime.Parse("2021-12-12"), Genre="Music", Place="Qeusarya", Description="Eyal Golan comes up with new singles",Urlimage="https://www.israelhayom.co.il/wp-content/uploads/2021/05/72336_KOKX2040_2018-07-26-1920x960.jpg" },
                new Event{ArtistName="Maccabi Haifa", AvailableTickets=30000,  Date=DateTime.Parse("2021-12-01"), Genre="Sport", Place="Sammy-Offer stadium, Haifa", Description="Maccabi Haifa vs Maccabi TLV",Urlimage="https://upload.wikimedia.org/wikipedia/he/thumb/0/0c/%D7%A1%D7%9E%D7%9C_%D7%9E%D7%9B%D7%91%D7%99_%D7%97%D7%99%D7%A4%D7%94_2020.png/800px-%D7%A1%D7%9E%D7%9C_%D7%9E%D7%9B%D7%91%D7%99_%D7%97%D7%99%D7%A4%D7%94_2020.png"},
                new Event{ArtistName="Pixies", AvailableTickets=1400,  Date=DateTime.Parse("2022-01-01"), Genre="Music", Place="Barbi, TLV", Description="The Greatest rock band with first new year concert!",Urlimage="https://static.wikia.nocookie.net/winx/images/b/b2/Winx_Club_Layla_and_the_Pixies.png/revision/latest/scale-to-width-down/352?cb=20210413215200"},
                new Event{ArtistName="Sarit Hadad", AvailableTickets=1700, Date=DateTime.Parse("2021-10-16"), Genre="Music", Place="Rishon Lezion", Description="The Middle East queen with a brand new show",Urlimage="https://kaspit-art.co.il/wp-content/uploads/2018/05/%D7%A9%D7%A8%D7%99%D7%AA-%D7%97%D7%93%D7%93-1.png"}
            };

            foreach (Event e in events)
            {
                context.Event.Add(e);
            }

            context.SaveChanges();

            if (context.Tickets.Any()) { return; }

            var tickets = new Models.Ticket[] {
                new Models.Ticket{Description="Omer Adam show", Price=200, Available=true, EventID=events[0].Id,Event=events[0]},
                new Ticket{Description="Eyal Golan show", Price=180, Available=true, EventID=events[1].Id,Event=events[1]},
                new Ticket{Description="Maccabi Haifa vs Maccabi TLV", Price=80, Available=true, EventID=events[2].Id,Event=events[1]},
                new Ticket{Description="Pixies show", Price=100, Available=true, EventID=events[3].Id,Event=events[1]},
                new Ticket{Description="Sarit Hadad show", Price=100, Available=true, EventID=events[4].Id,Event=events[4]}
            };

            foreach (Ticket t in tickets)
            {
                context.Tickets.Add(t);
            }

            context.SaveChanges();

        }

    }
}
