using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
