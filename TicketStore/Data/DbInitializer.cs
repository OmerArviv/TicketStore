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
            var tickets = new List<Ticket>();
            var users = new Models.User[]
            {
                new Models.User{Birthdate=DateTime.Parse("2021-05-17"), Email="rrttou@gmail.com", FirstName="Raz",
                    Gender=0, LastName="Yaniv", Password="Admin@123", PasswordConfirm="Admin@123",
                    UserName="rrttou", Type=0, IsAdmin=true, Tickets=tickets }


            };
            foreach (User u in users)
            {
                context.User.Add(u);
            }
            context.SaveChanges();

            if (context.Event.Any()) { return; }

            var events = new Models.Event[] {
                new Models.Event{
                    ArtistName="Omer Adam",
                    AvailableTickets=1000,
                    Date=DateTime.Parse("2021-11-11"),
                    Genre="Music",
                    Place="Qeusarya",
                    Description="Last Omer Adam show of 2021!",
                    ImageUrl="https://i.ytimg.com/vi/HXUQnKKw8K8/maxresdefault.jpg",
                    ImageUrl2="https://www.reali.co.il/wp-content/uploads/2018/09/%D7%A2%D7%95%D7%9E%D7%A8%D7%90%D7%93%D7%9D.jpg",
                    ImageUrl3="https://www.kan-ashkelon.co.il/wp-content/uploads/2021/07/66d7045c52c29e31c64af73400dc6ff7-845x563.png",
                    stars=5,
                    MinPrice=199,
                    LocationX = 32.496026, 
                    Locationy = 34.891230
                },
                new Event{
                    ArtistName="Eyal Golan",
                    AvailableTickets=2500,
                    Date=DateTime.Parse("2021-12-12"),
                    Genre="Music",
                    Place="Qeusarya",
                    Description="Eyal Golan comes up with new singles",
                    ImageUrl="https://www.kzat-tarbut.com/wp-content/uploads/2018/09/2018-8211-MGBBnM8s9D8-1024x576.jpg",
                    ImageUrl2="data:image/jpeg;base64,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",
                    ImageUrl3="https://www.ynet.co.il/PicServer5/2017/01/01/7489475/74894680100791640360no.jpg",
                    stars=3,
                    MinPrice=149,
                    LocationX = 32.496026,
                    Locationy = 34.891230
                },
                new Event{
                    ArtistName="Maccabi Haifa",
                    AvailableTickets=30000,
                    Date=DateTime.Parse("2021-12-01"),
                    Genre="Sport",
                    Place="Sammy-Offer stadium, Haifa",
                    Description="Maccabi Haifa vs Maccabi TLV",
                    ImageUrl="https://www.israelhayom.co.il/wp-content/uploads/2021/05/ALEN86499-e1622054286836-960x640.jpg",
                    ImageUrl2="https://sport1images.maariv.co.il/image/upload/f_auto,fl_lossy,c_thumb,g_north,w_728,h_441/1031474",
                    ImageUrl3="https://img.haarets.co.il/img/1.9683928/2259543329.jpg?precrop=1488,1486,x303,y25&height=1200&width=1200",
                    stars=2,
                    MinPrice=69,
                    LocationX = 32.783211, 
                    Locationy = 34.965248

                },
                new Event{
                    ArtistName="Hapoel Beer Sheva",
                    AvailableTickets=30000,
                    Date=DateTime.Parse("2021-12-10"),
                    Genre="Sport",
                    Place="Terner stadium, Beer Sheva",
                    Description="Hapoel Beer Sheva vs Maccabi TLV ",
                    ImageUrl="https://sport1images.maariv.co.il/image/upload/f_auto,fl_lossy,c_thumb,g_north/1024488",
                    ImageUrl2="https://sport1images.maariv.co.il/image/upload/f_auto,fl_lossy,c_thumb,g_north,w_728,h_441/1002630",
                    ImageUrl3="https://a7.org/pictures/1037/1037898.jpg",
                    stars=2,
                    MinPrice=69,
                    LocationX = 31.273196,
                    Locationy = 34.779681

                },
                new Event{
                    ArtistName="Pixies",
                    AvailableTickets=1400,
                    Date=DateTime.Parse("2022-01-01"),
                    Genre="Music",
                    Place="Barbi, TLV",
                    Description="The Greatest rock band with first new year concert!",
                    ImageUrl="https://tikair.co.il/storage/2021/03/pixies-cover.jpg",
                    ImageUrl2="https://img.wcdn.co.il/f_auto,q_auto,w_1200,t_54/2/9/5/6/2956387-46.jpg",
                    ImageUrl3="data:image/jpeg;base64,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",
                    stars=4,
                    MinPrice=99,
                    LocationX = 32.051282,
                    Locationy = 34.769307
                },
                new Event{
                    ArtistName="Sarit Hadad",
                    AvailableTickets=1700,
                    Date=DateTime.Parse("2021-10-16"),
                    Genre="Music",
                    Place="Amphi Shuni, Binyamina",
                    Description="The Middle East queen with a brand new show",
                    ImageUrl="https://grayclub.co.il/wp-content/uploads/2019/08/%D7%A9%D7%A8%D7%99%D7%AA-%D7%97%D7%93%D7%93.png",
                    ImageUrl2="data:image/jpeg;base64,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",
                    ImageUrl3="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT-MX15rhT3pVGtqMHicfZOhH67CvRDZ3kaKA&usqp=CAU",
                    stars=4,
                    MinPrice=79,
                    LocationX = 31.996655, 
                    Locationy = 34.736217

                },
                 new Event{
                    ArtistName="No Time to Die",
                    AvailableTickets=400,
                    Date=DateTime.Parse("2021-10-16"),
                    Genre="Movie",
                    Place="Cinema City, Netanya",
                    Description="The latest James Bond movie",
                    ImageUrl="https://m.media-amazon.com/images/M/MV5BYWQ2NzQ1NjktMzNkNS00MGY1LTgwMmMtYTllYTI5YzNmMmE0XkEyXkFqcGdeQXVyMjM4NTM5NDY@._V1_.jpg",
                    ImageUrl2="https://i.insider.com/5e57d0c1fee23d37c7514919?width=700",
                    ImageUrl3="https://ychef.files.bbci.co.uk/976x549/p09x617v.jpg",
                    stars=4,
                    MinPrice=39,
                    LocationX = 31.984473,
                    Locationy = 34.769859

                 },
                 new Event{
                    ArtistName="Harry Potter and the Sorcerer's Stone",
                    AvailableTickets=400,
                    Date=DateTime.Parse("2021-10-18"),
                    Genre="Movie",
                    Place="Yes Planet, Haderah",
                    Description="The new movie by J.K.Rolling",
                    ImageUrl="https://images.sellbrite.com/production/152272/P5159/2b4f2cc8-2173-5339-a58b-51fc06fb0919.jpg",
                    ImageUrl2="https://m.media-amazon.com/images/M/MV5BNjhlM2UyYTEtOThkZS00MTAyLWFlOGQtNzIwNTc5ZmVkOWQwXkEyXkFqcGdeQXVyNzU1NzE3NTg@._V1_.jpgO",
                    ImageUrl3="https://cdn.mos.cms.futurecdn.net/c630d2e738d3bb015c33a5a338108b21.jpg",
                    stars=3,
                    MinPrice=39,
                    LocationX = 31.984473,
                    Locationy = 34.769859

                 }
            };

            foreach (Event e in events)
            {
                context.Event.Add(e);
            }


            context.SaveChanges();

            /*if (context.Tickets.Any()) { return; }

            var tickets = new Models.Ticket[] {
                new Models.Ticket{Description="Omer Adam show", Price=199, Available=true, EventID=events[0].Id,Event=context.Event.FirstOrDefault(c=>c.Id==events[0].Id)},
                new Ticket{Description="Eyal Golan show", Price=149, Available=true, EventID=events[1].Id,Event=context.Event.FirstOrDefault(c=>c.Id==events[1].Id)},
                new Ticket{Description="Maccabi Haifa vs Maccabi TLV", Price=69, Available=true, EventID=events[2].Id,Event=context.Event.FirstOrDefault(c=>c.Id==events[2].Id)},
                new Ticket{Description="Pixies show", Price=99, Available=true, EventID=events[3].Id,Event=context.Event.FirstOrDefault(c=>c.Id==events[3].Id)},
                new Ticket{Description="Sarit Hadad show", Price=79, Available=true, EventID=events[4].Id,Event=context.Event.FirstOrDefault(c=>c.Id==events[4].Id)}
            };

            foreach (Ticket t in tickets)
            {
                context.Tickets.Add(t);
            }

            context.SaveChanges();*/

        }

    }
}
