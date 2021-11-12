using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TicketStore.Data;
using TicketStore.Models;
using Tweetinvi;

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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login");
        }


        public async Task<IActionResult> Twitter()
        {
            
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Twitter(String tweet)
        {
            string APIkey = "r2TfeRi4hFBAuVcxU1NgUw9Vx";
            string APIsecret = "bWWJkrTDdhfmn7oab3CsSYiu8PvLJw7lx8zh4yprM5QbjlNo4K";
            string APIToken = "1459045772188606486-JnnsEcDOj44R3MN2nHvx9J0mx5Irs5";
            string APITokenSecret = "fQN9bMOYnzvMbwzlqhFu3cvsYX3idKIzuSgg4aq5axjME";
            string APIBearerToken = "AAAAAAAAAAAAAAAAAAAAANTrVgEAAAAAaFH2KZfe2eUoqldVXuuVr6GgC2k%3DWqfviCgFXRhuR1BC1xwD2fFdV1tOkBLjqIn6krTh087V2Fghry";
            var client = new TwitterClient(APIkey, APIsecret, APIToken, APITokenSecret);
            //client.Config.TweetMode = TweetMode.Compat;
            await client.Tweets.PublishTweetAsync(tweet);
            return View();
        }

        public IActionResult About()
        {
            return View("About");
        }
        public IActionResult Contact()
        {
            return View("Contact");
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
