using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using org.apache.zookeeper.data;
using TicketStore.Data;
using TicketStore.Models;
using System.Collections.ObjectModel;

namespace TicketStore.Controllers
{
    public class EventsController : Controller
    {
        private readonly ShowContext _context;

        public EventsController(ShowContext context)
        {
            _context = context;
        }


        public ActionResult Buy(int? id,int Quan)
        {
            if (!User.Claims.Any())
            {
                return View("NotLogIn");
            }
            var e = _context.Event.Where(temp => temp.Id == id).FirstOrDefault();
            var tmp = User.Claims.First(c => c.Type == "UserId").Value;
            var idd = int.Parse(tmp);
            //var tempUser = _context.User.FirstOrDefault();
            var raz = from t in _context.User where idd == t.Id select t;
            var user = raz.FirstOrDefault();
            int totalAmount = 0;
            //var raz = _context.User.FirstOrDefault();
            if (e.AvailableTickets >= Quan)
            {
                ICollection<Ticket> tick = new List<Ticket>();
                e.AvailableTickets = e.AvailableTickets - Quan;
                for (int i = 0; i < Quan; i++)
                {
                    Ticket tempTicket = new Ticket { Description = e.Description, Price = e.MinPrice, Available = false, EventID = (int)id, Event = _context.Event.FirstOrDefault(c => c.Id == id) };
                    
                   
                    if (user.Tickets == null)
                        user.Tickets = new List<Ticket>();
                    var tmpEvent = _context.Event.FirstOrDefault(e => e.Id == id);
                    tempTicket.Event = tmpEvent;
                    user.Tickets.Add(tempTicket);
                    tempTicket.UserID = user.Id;
                    tempTicket.Costumer = user;
                    totalAmount += tempTicket.Price;
                    tick.Add(tempTicket);
                    int j = 1; //for debug
                }
                var order = new Order
                {
                    Costumer = user,
                    Event = e,
                    EventId = e.Id,
                    UserId = user.Id,
                    NumOfTickets = Quan,
                    OrderTime = DateTime.Now,
                    TotalAmount = totalAmount
                   
                };
                _context.Order.Add(order);
                
               // _context.SaveChanges();
                foreach (Ticket t in tick)
                {
                    _context.Tickets.Add(t);
                }
                _context.SaveChanges();

                return View(tick.FirstOrDefault().Costumer);
            }
            else
                return Redirect("https://localhost:44350/events/");
        }
        public async Task<IActionResult> FilterBy(string ArtistName, string Place, string Genre)
        {
            var result = from e
                         in _context.Event
                         select e;
            
            if (!(String.IsNullOrEmpty(ArtistName)))
            {
                result = from eve in result
                         where eve.ArtistName.Equals(ArtistName)
                         select eve;
            }
            if (!(String.IsNullOrEmpty(Place)))
            {
                result = from eve in result
                         where eve.Place.Equals(Place)
                         select eve;
            }
            if (!(String.IsNullOrEmpty(Genre)))
            {
                result = from eve in result
                         where eve.Genre.Equals(Genre)
                         select eve;
            }
            if (result == null)
            {
                return View("Index");
            }
            return View("Index", await result.ToListAsync());

        }
        public ActionResult Summary(int? id)
        {
            if (id != null)
            {
                var e = _context.Event.Where(temp => temp.Id == id); 
                if(e.FirstOrDefault() != null)
                {
                    return View(e.FirstOrDefault());
                } else
                {
                    return View("NotFound");
                }
             
            }
            else
                return View("NotFound");
        }

        // GET: Events
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            var events = from e in _context.Event select e;

            List<string> places = new List<string>();
            List<string> names = new List<string>();
            List<string> genre = new List<string>();

            foreach (var e in _context.Event)
            {
                if (!places.Contains(e.Place))
                {
                    places.Add(e.Place);
                }
                if (!names.Contains(e.ArtistName))
                {
                    names.Add(e.ArtistName);
                }
                if (!genre.Contains(e.Genre))
                {
                    genre.Add(e.Genre);
                }


            }
            
            var tmp = new List<SelectListItem>();
                    foreach (var p in places)
                    {
                        tmp.Add (new SelectListItem { Text = p, Value = p });
                    }
            var listPlace = tmp;

            tmp = new List<SelectListItem>();
            foreach (var name in names)
            {
                tmp.Add(new SelectListItem { Text = name, Value = name });
            }
            var listName = tmp;

            tmp = new List<SelectListItem>();
            foreach (var g in genre)
            {
                tmp.Add(new SelectListItem { Text = g, Value = g });
            }
            var listGenre = tmp;

            ViewData["ListName"] = listName;
            ViewData["ListPlace"] = listPlace;
            ViewData["ListGenre"] = listGenre;

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(myEvent => myEvent.ArtistName.Contains(searchString));
                
            }

            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(s => s.ArtistName);
                    break;
                case "Date":
                    events = events.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(s => s.Date);
                    break;
                default:
                    events = events.OrderBy(s => s.Genre);
                    break;
            }
            

            return View(await events.ToListAsync());


        }


        public async Task<IActionResult> Filter(string filterByGenre)
        {
            var events = from e in _context.Event select e;
            
            List<string> places = new List<string>();
            List<string> names = new List<string>();
            List<string> genre = new List<string>();

            foreach (var e in _context.Event)
            {
                if (!places.Contains(e.Place))
                {
                    places.Add(e.Place);
                }
                if (!names.Contains(e.ArtistName))
                {
                    names.Add(e.ArtistName);
                }
                if (!genre.Contains(e.Genre))
                {
                    genre.Add(e.Genre);
                }


            }

            var tmp = new List<SelectListItem>();
            foreach (var p in places)
            {
                tmp.Add(new SelectListItem { Text = p, Value = p });
            }
            var listPlace = tmp;

            tmp = new List<SelectListItem>();
            foreach (var name in names)
            {
                tmp.Add(new SelectListItem { Text = name, Value = name });
            }
            var listName = tmp;

            tmp = new List<SelectListItem>();
            foreach (var g in genre)
            {
                tmp.Add(new SelectListItem { Text = g, Value = g });
            }
            var listGenre = tmp;

            ViewData["ListName"] = listName;
            ViewData["ListPlace"] = listPlace;
            ViewData["ListGenre"] = listGenre;


            if (filterByGenre != null)
            {
                var res = from e in events where e.Genre.Equals(filterByGenre) select e;
                if (res != null)
                {
                    return View("Index", await res.ToListAsync());
                }
            }

            return View("Index", events.ToListAsync());


        }
        [HttpPost]
        public async Task<IActionResult> Filter(string ListName, string ListPlace, string ListGenre)
        {
            var events = from e in _context.Event select e;


            List<string> places = new List<string>();
            List<string> names = new List<string>();
            List<string> genre = new List<string>();

            foreach (var e in _context.Event)
            {
                if (!places.Contains(e.Place))
                {
                    places.Add(e.Place);
                }
                if (!names.Contains(e.ArtistName))
                {
                    names.Add(e.ArtistName);
                }
                if (!genre.Contains(e.Genre))
                {
                    genre.Add(e.Genre);
                }


            }

            var tmp = new List<SelectListItem>();
            foreach (var p in places)
            {
                tmp.Add(new SelectListItem { Text = p, Value = p });
            }
            var listPlace = tmp;

            tmp = new List<SelectListItem>();
            foreach (var name in names)
            {
                tmp.Add(new SelectListItem { Text = name, Value = name });
            }
            var listName = tmp;

            tmp = new List<SelectListItem>();
            foreach (var g in genre)
            {
                tmp.Add(new SelectListItem { Text = g, Value = g });
            }
            var listGenre = tmp;

            ViewData["ListName"] = listName;
            ViewData["ListPlace"] = listPlace;
            ViewData["ListGenre"] = listGenre;

            var result = from e
                         in _context.Event
                         select e;

            if (!(String.IsNullOrEmpty(ListName)))
            {
                result = from eve in result
                         where eve.ArtistName.Equals(ListName)
                         select eve;
            }
            if (!(String.IsNullOrEmpty(ListPlace)))
            {
                result = from eve in result
                         where eve.Place.Equals(ListPlace)
                         select eve;
            }
            if (!(String.IsNullOrEmpty(ListGenre)))
            {
                result = from eve in result
                         where eve.Genre.Equals(ListGenre)
                         select eve;
            }
            if (result.FirstOrDefault() == null)
            {
                return View("Index", await events.ToListAsync());
            }
            return View("Index", await result.ToListAsync());
        }
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }

            if (id == null)
            {
                return View("NotFound");
            }

            var event1 = await _context.Event.Include(t=>t.Tickets)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (event1 == null)
            {
                return View("NotFound");
            }
            return View( event1);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public string Index(Event @event)
        {
            return null;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistName,Place,AvailableTickets,Genre,Date,stars,ImageUrl,ImageUrl2,ImageUrl3,MinPrice,Description,LocationX,LocationY")] Event @event)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (id == null)
            {
                return View("NotFound");
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return View("NotFound");
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistName,Place,AvailableTickets,Genre,Date,stars,ImageUrl,ImageUrl2,ImageUrl3,MinPrice,Description,LocationX,LocationY")] Event @event)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (id != @event.Id)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            {
                return View("NotFound");
            }
            if (id == null)
            {
                return View("NotFound");
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return View("NotFound");
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }

        public ActionResult Statistics()
        {
            //if (!(User.Claims.Any() && User.Claims.First(c => c.Type == "Role").Value.Equals("Admin")))
            //{
            //    return View("NotFound");
            //}

            ICollection<Stat> statistic1 = new Collection<Stat>();
            ICollection<Stat> statistic2 = new Collection<Stat>();
            Dictionary<string, int> dic1 = new Dictionary<string, int>();
            Dictionary<string, int> dic2 = new Dictionary<string, int>();
            dic1.Add("Sport", 0);
            dic1.Add("Movie", 0);
            dic1.Add("Music", 0);
            dic2.Add("Sport", 0);
            dic2.Add("Movie", 0);
            dic2.Add("Music", 0);

            foreach (var t in _context.Tickets)
            {
                if (t == null)
                    continue;
                var tmpEvent = _context.Event.FirstOrDefault(e => e.Id == t.EventID);
                if (!dic1.ContainsKey(tmpEvent.Genre))
                {
                    dic1.Add(t.Event.Genre, 1);
                }
                else
                {
                    int temp = dic1.GetValueOrDefault(t.Event.Genre);
                    dic1.Remove(tmpEvent.Genre);
                    dic1.Add(t.Event.Genre, temp + 1);
                }
            }



            foreach (var v in dic1)
            {
                statistic1.Add(new Stat(v.Key, v.Value));
            }

            ViewBag.data1 = statistic1;
            //////////////////////////// STAT 2

            foreach (var t in _context.Tickets)
            {
                if (t == null)
                    continue;
                var tmpEvent = _context.Event.FirstOrDefault(e => e.Id == t.EventID);
                if (!dic2.ContainsKey(tmpEvent.Genre))
                {
                    dic2.Add(t.Event.Genre, t.Price);
                }
                else
                {
                    int temp = dic2.GetValueOrDefault(t.Event.Genre);
                    dic2.Remove(tmpEvent.Genre);
                    dic2.Add(t.Event.Genre, temp + t.Price);
                }
            }
            foreach (var v in dic2)
            {
                statistic2.Add(new Stat(v.Key, v.Value));
            }
            return View(statistic2);
        }
    }
    public class Stat
    {
        public string Key;
        public int Values;
        public Stat(string key, int values)
        {
            Key = key;
            Values = values;
        }
    }

}


