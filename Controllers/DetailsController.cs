using foiEPP.Data;
using foiEPP.Models;
using foiEPP.Viewmodels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Controllers
{
    public class DetailsController : Controller
    {
        private readonly FacultyContext _context;
        public DetailsController(FacultyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Record()
        {
            string loggedEmail = HttpContext.Session.GetString("email");
            User loggedUser = _context.Users.Where(student => student.Email == loggedEmail).First();
            RecordViewModel viewData = new RecordViewModel();
            viewData.Rooms = GetRooms();
            viewData.Classes = GetClasses(loggedUser.ID);
            return View(viewData);
        }

        [HttpPost]
        [ActionName("Record")]
        public IActionResult RecordPost(int classID, int roomID, int time)
        {
            List<StudentWithImageViewModel> students = null;
            if (time != 0)
            {
                Record rec = _context.Records.Where(r => r.ID == time).First();
                List<Record> records = _context.Records.Where(record => record.Time.Year == rec.Time.Year && record.Time.Month == rec.Time.Month && record.Time.Day == rec.Time.Day && record.Time.Hour == rec.Time.Hour && record.Time.Minute == rec.Time.Minute && record.Time.Second == rec.Time.Second).ToList();
                students = new List<StudentWithImageViewModel>();
                foreach(var record in records)
                {
                    User student = _context.Users.Where(s => s.ID == record.UserID).First();
                    StudentWithImageViewModel studentRecord = new StudentWithImageViewModel();
                    studentRecord.Student = student;
                    studentRecord.Image = record.Image;
                    students.Add(studentRecord);
                }
            }
            string loggedEmail = HttpContext.Session.GetString("email");
            User loggedUser = _context.Users.Where(student => student.Email == loggedEmail).First();
            RecordViewModel viewData = new RecordViewModel();
            viewData.Rooms = GetRooms();
            viewData.Classes = GetClasses(loggedUser.ID);
            List<Record> allRecords = _context.Records.Where(record => record.ClassID == classID && record.RoomID == roomID).ToList();
            List<Record> viewRecords = allRecords.GroupBy(x => x.Time.Ticks).Select(x => x.FirstOrDefault()).OrderByDescending(y => y.Time).ToList();
            viewData.Records = viewRecords;
            ViewBag.Students = students;
            return View(viewData);
        }

        private List<Class> GetClasses(int userID)
        {
            List<int> classIDs = _context.UserClasses.Where(uc => uc.UserID == userID).Select(s => s.ClassID).ToList();
            List<Class> classes = _context.Classes.Where(c => classIDs.Contains(c.ID)).ToList();
            return classes;
        }

        private List<Room> GetRooms()
        {
            List<Room> rooms = _context.Rooms.ToList();
            return rooms;
        }
    }
}
