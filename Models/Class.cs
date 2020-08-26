using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Models
{
    public class Class
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<UserClass> UserClasses { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
