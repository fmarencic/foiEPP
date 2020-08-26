using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserTypeID { get; set; }
        public UserType UserType { get; set; }
        public ICollection<UserImages> UserImages { get; set; }
        public ICollection<UserClass> UserClasses { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
