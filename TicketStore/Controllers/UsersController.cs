using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketStore.Data;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    public class UsersController : Controller
    {
        private readonly ShowContext _context;

        public UsersController(ShowContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }


        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,FirstName,LastName,Password,PasswordConfirm,Email,Birthdate,Gender,Type,CartId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



        //Register
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Id,UserName,FirstName,LastName,Password,PasswordConfirm,Email,Birthdate,Gender,Type,CartId")] User user)
        {

            // Validates the input data
            if (user.FirstName == null || user.LastName == null || user.Email == null || user.Password == null || user.PasswordConfirm == null)
            {
                ViewData["error"] = "Please make sure you enter data to all the fields.";
                return View();
            }

            // Check if the password matches the repeated one
            if (user.Password != user.PasswordConfirm)
            {
                ViewData["error"] = "Passwords don't match.";
                return View();
            }

            // Check if account already exists
            if (_context.User.FirstOrDefault(u => u.Email == user.Email) != null)
            {
                ViewData["error"] = "Email already exists.";
                return View();
            }
            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.UserName == user.UserName); //checking if there is username with the same name it will return null if doesnt, if there is the object
                if (q == null)
                {
                    //creating a new cart for this user
                   // Cart cart = new Cart();
                   
                    //cart.LastUpdate = DateTime.Now;
                    //_context.Add(cart);
                    //_context.Add(user);
                    //await _context.SaveChangesAsync();

                    //user.CartId = cart.Id;
                    //cart.UserId = user.Id;
                    

                    await _context.SaveChangesAsync();
                    var u = _context.User.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
                    Signin(u);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Unable to comply; cannot register this user";
                }
            }
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Login()
        {
            return View();
        }

        //AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        //login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Id,Email,Password")] User user, string ReturnUrl)
        {
          //  var q = _context.User.FirstOrDefault(u => u.Username == user.UserName);
            var q = from u in _context.User
                   where u.Email == user.Email &&
                    u.Password == user.Password                    select u;
            //   same same             var q = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password); //checking if there is username with the same name it will return null if doesnt, if there is the object
            if (q.Count() > 0)
            {
                //HttpContext.Session.SetString("username", q.First().Username);
                Signin(q.First());

            }
            else
            {
                ViewData["Error"] = "Username and/or password are incorrect";
                return View();
            }
            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        //Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index), "Home");
        }


        //Signin
        /**
         * Sign in function that starting 10 minutes session and saving name,id,cartId and mail as claims.
         */
        private async void Signin(User account)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, account.Email),
            new Claim("FullName", account.FirstName),
            new Claim(ClaimTypes.Role, account.Type.ToString()),
            new Claim("UserId", account.Id.ToString()),
            new Claim("cartId", account.CartId.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }


        /**
         * Search user fuction that return list of users with the input string in their 
         * name,email or username.
         */
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Search(string input)
        {
            if (input != null)
            {
                return PartialView(await _context.User.Where(a => (a.UserName.Contains(input) ||
                a.Email.Contains(input) ||
                a.FirstName.Contains(input) ||
                a.LastName.Contains(input)
                )).ToListAsync());
            }
            else
            {
                return PartialView(await _context.User.ToListAsync());
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null || id == null)
            {
                return NotFound();

            }
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}

