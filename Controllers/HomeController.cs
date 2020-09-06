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
using foiEPP.Helpers;

namespace foiEPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FacultyContext _context;
        private FaceRecognitionHelper faceRecognitionHelper;

        public HomeController(ILogger<HomeController> logger, FacultyContext context)
        {
            _logger = logger;
            _context = context;
            faceRecognitionHelper = new FaceRecognitionHelper(context);
        }

        /**
         * Loads after login.
         * Checks if user is logged in and checks users type and redirects depend on it.
         */
        public IActionResult Index()
        {
            // faceRecognitionHelper.AddNewFaces();
            if (!HttpContext.Session.Keys.Contains("email"))
            {
                return RedirectToAction("Login", "Login");
            }
            if (HttpContext.Session.GetString("type") == "1")
            {
                ClassesRoomsViewModel viewData = new ClassesRoomsViewModel();
                viewData.Rooms = GetRooms();
                viewData.Classes = GetClasses();
                return View(viewData);
            }
            else if(HttpContext.Session.GetString("type") == "2")
                return RedirectToAction("Record", "Details");
            return View();
        }

        /**
         * Gets class, room and image to recognize students from.
         * Calls function for image recognition and stores students in DB.
         */
        [HttpPost]
        public async Task<IActionResult> Record(int classID, int roomID, IFormFile image)
        {
            Room room = _context.Rooms.Where(r => r.ID == roomID).FirstOrDefault();
            Class facClass = _context.Classes.Where(r => r.ID == classID).FirstOrDefault();
            List<StudentWithImageViewModel> recognizedStudents = new List<StudentWithImageViewModel>();
            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                ViewBag.Image = fileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\uploaded", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                recognizedStudents = faceRecognitionHelper.RecognizeStudents(filePath);
            }
            DateTime current = DateTime.Now;
            current = new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second, current.Kind);
            foreach (var student in recognizedStudents)
            {
                Record record = new Record();
                record.RoomID = roomID;
                record.ClassID = classID;
                record.UserID = student.Student.ID;
                record.Time = current;
                record.Image = student.Image;
                _context.Records.Add(record);
            }
            _context.SaveChanges();
            ViewBag.Time = current;
            ViewBag.RoomID = room.ID;
            ViewBag.ClassID = facClass.ID;
            ViewBag.Title = facClass.Name + " - " + room.Name;
            ViewBag.People = recognizedStudents;
            return View();
        }

        /**
         * Adds non-recognized students to record.
         */
        [HttpPost]
        public IActionResult AddStudents(UserViewModel studentsPassed)
        {
            foreach (var student in studentsPassed.Students)
            {
                User newStudent = _context.Users.Where(st => st.FirstName == student.FirstName && st.LastName == student.LastName).First();
                Record newRecord = new Record();
                newRecord.ClassID = studentsPassed.ClassID;
                newRecord.RoomID = studentsPassed.RoomID;
                newRecord.Time = studentsPassed.Time;
                newRecord.UserID = newStudent.ID;
                _context.Records.Add(newRecord);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /**
         * Gets all classes
         */
        private List<Class> GetClasses()
        {
            List<Class> classes = _context.Classes.ToList();
            return classes;
        }

        /**
         * Gets all rooms
         */
        private List<Room> GetRooms()
        {
            List<Room> rooms = _context.Rooms.ToList();
            return rooms;
        }
    }
}
