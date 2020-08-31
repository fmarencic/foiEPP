using foiEPP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Viewmodels
{
    public class StudentWithImageViewModel
    {
        public byte[] Image { get; set; }
        public User Student { get; set; }
    }
}
