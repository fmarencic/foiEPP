using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using foiEPP.Models;
using foiEPP.Data;
using foiEPP.Viewmodels;
using System.IO;

namespace foiEPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FacultyContext _context;

        public HomeController(ILogger<HomeController> logger, FacultyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("email"))
            {
                return RedirectToAction("Login", "Login");
            }
            if(HttpContext.Session.GetString("type") == "2")
            {
                ClassesRoomsViewModel viewData = new ClassesRoomsViewModel();
                viewData.Rooms = GetRooms();
                viewData.Classes = GetClasses();
                return View(viewData);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Record(int classID, int roomID, IFormFile image)
        {
            Room room = _context.Rooms.Where(r => r.ID == roomID).FirstOrDefault();
            Class facClass = _context.Classes.Where(r => r.ID == classID).FirstOrDefault();
            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                ViewBag.Image = fileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\uploaded", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }
            ViewBag.Title = facClass.Name + " - " + room.Name;
            ViewBag.People = new List<RecognizedStudentsViewModel>();
            return View();
        }

        [HttpPost]
        public IActionResult AddStudents(UserViewModel studentsPassed)
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Class> GetClasses()
        {
            List<Class> classes = _context.Classes.ToList();
            return classes;
        }

        private List<Room> GetRooms()
        {
            List<Room> rooms = _context.Rooms.ToList();
            return rooms;
        }
    }
}
