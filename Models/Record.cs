using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Models
{
    public class Record
    {
        public int ID { get; set; }
        public int ClassID { get; set; }
        public int RoomID { get; set; }
        public int UserID { get; set; }
        public DateTime Time { get; set; }
        public string Encoding { get; set; }
        public Class Class { get; set; }
        public Room Room { get; set; }
        public User User { get; set; }
    }
}
