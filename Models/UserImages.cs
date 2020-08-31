using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Models
{
    public class UserImages
    {
        public int ID { get; set; }
        public byte[] Encoding { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
