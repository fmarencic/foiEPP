using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Models
{
    public class UserClass
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
        public User User { get; set; }
    }
}
