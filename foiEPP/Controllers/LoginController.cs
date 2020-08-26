using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using foiEPP.Data;
using foiEPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace foiEPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly FacultyContext _context;
        public LoginController(FacultyContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Validate(User user)
        {
            var dbUser = _context.Users.Where(u => u.Email == user.Email);
            if (dbUser.Any())
            {
                if (dbUser.Where(u => u.Password == user.Password).Any())
                {
                    HttpContext.Session.SetString("email", user.Email);
                    HttpContext.Session.SetString("name", user.FirstName + " " + user.LastName);
                    HttpContext.Session.SetString("type", dbUser.FirstOrDefault().UserTypeID.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Neispravna lozinka";
                    return View("Login");
                }
            }
            else
            {
                ViewBag.Message = "Neispravan email";
                return View("Login");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("name");
            return View("Login");
        }
    }
}
