using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("LoggedInUser", newUser.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");

        }

        [HttpPost("login")]
        public IActionResult Login(LogUser LoginUser)
        {
            if(ModelState.IsValid)
            {
                User userInDB = dbContext.Users.FirstOrDefault(u => u.Email == LoginUser.LEmail);
                if(userInDB == null)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Index");
                }
                
                var hasher = new PasswordHasher<LogUser>();
                var result = hasher.VerifyHashedPassword(LoginUser, userInDB.Password, LoginUser.LPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("LoggedInUser", userInDB.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");

        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? session = HttpContext.Session.GetInt32("LoggedInUser");
            if(session == null)
            {
                return RedirectToAction("Index");
            }
            List<DojoActivity> AllActivities= dbContext.DojoActivities
            .Include(t =>t.Creator)
            .Include(g => g.Guest)
            .ThenInclude(u => u.User)
            .OrderBy(c => c.ActivityDate)
            .ToList();
            User currentUser= dbContext.Users.FirstOrDefault(u => u.UserId == (int)HttpContext.Session.GetInt32("LoggedInUser") );
            ViewBag.CurrentUser= currentUser;
            return View(AllActivities);

        }
        [HttpGet("newactivity")]
        public IActionResult NewActivity()
        {
            int? session = HttpContext.Session.GetInt32("LoggedInUser");
            if(session == null)
            {
                return RedirectToAction("Index");
            }
            return View();
       
        }
        [HttpPost("createactivity")]
        public IActionResult CreateActivity(DojoActivity newActivity)
        {
            int? session = HttpContext.Session.GetInt32("LoggedInUser");
            if(ModelState.IsValid)
            {
                if(newActivity.ActivityDate <= DateTime.Today)
                {
                    ModelState.AddModelError("ActivityDate","Plan date must be a future date");
                    return View ("NewActivity");
                }
                newActivity.UserId = (int)HttpContext.Session.GetInt32("LoggedInUser");
                dbContext.Add(newActivity);
                dbContext.SaveChanges();
                DojoActivity One= dbContext.DojoActivities.LastOrDefault();
                return Redirect("dojoactivityinfo/" + One.DojoActivityId);
            }
            return View("NewActivity");
        }
        [HttpGet("dojoactivityinfo/{DojoActivityId}")]
        public IActionResult DojoActivityInfo(int DojoActivityId)
        {
            int? session = HttpContext.Session.GetInt32("LoggedInUser");
            if(session == null)
            {   
                return RedirectToAction("Index");
            }
            DojoActivity One = dbContext.DojoActivities
            .Include(c => c.Creator)
            .Include(a => a.Guest)
            .ThenInclude(b => b.User)
            .FirstOrDefault(c => c.DojoActivityId == DojoActivityId);
            User currentUser= dbContext.Users.FirstOrDefault(u => u.UserId == (int)HttpContext.Session.GetInt32("LoggedInUser") );
            ViewBag.CurrentUser= currentUser;
            return View(One);
            
            
        }

        [HttpGet("delete/{DojoActivityId}")]
        public IActionResult Delete(int DojoActivityId)
        {
            int? session = HttpContext.Session.GetInt32("LoggedInUser");
            if(session == null)
            {
                return RedirectToAction("Index");
            }
            DojoActivity DojoActivityDelete = dbContext.DojoActivities
            .FirstOrDefault(w => w.DojoActivityId == DojoActivityId);
            dbContext.DojoActivities.Remove(DojoActivityDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");

        }
        [HttpPost("rsvp/{DojoActivityId}")]
        public IActionResult RSVP(int DojoActivityId, RSVP newRSVP)
        {
            int? session = HttpContext.Session.GetInt32("LoggedInUser");
            if(session == null)
            {
                return RedirectToAction("Index");
            }
            newRSVP.DojoActivityId= DojoActivityId;
            newRSVP.UserId = (int) HttpContext.Session.GetInt32("LoggedInUser");
            dbContext.Add(newRSVP);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");

        }
        [HttpPost("unrsvp/{DojoActivityId}")]
        public IActionResult UNRSVP(int DojoActivityId)
        {
            int? session = HttpContext.Session.GetInt32("LoggedInUser");
            if(session == null)
            {
                return RedirectToAction("Index");
            }
            RSVP RemoveRSVP= dbContext.RSVPs
            .Where(m => m.DojoActivityId == DojoActivityId)
            .FirstOrDefault(a => a.UserId == session);
            dbContext.Remove(RemoveRSVP);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");


        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }

}
