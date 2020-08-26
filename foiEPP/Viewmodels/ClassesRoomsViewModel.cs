using foiEPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Viewmodels
{
    public class ClassesRoomsViewModel
    {
        public int RoomID { get; set; }
        public int ClassID { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Class> Classes { get; set; }
    }
}
