using foiEPP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foiEPP.Data
{
    public class FacultyContext : DbContext
    {
        public FacultyContext(DbContextOptions<FacultyContext> options) : base(options)
        {

        }

        public DbSet<Record> Records { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClass> UserClasses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<UserImages> UserImages { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
    }
}
