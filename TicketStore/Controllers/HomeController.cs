using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TicketStore.Data;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShowContext _context;

 
        public HomeController(ILogger<HomeController> logger, ShowContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var events = from t in _context.Event select t;
            if (events.Any())
            {
                events = events.OrderBy(a => a.Date);
                return View(events.First());
            }
            return View();
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
