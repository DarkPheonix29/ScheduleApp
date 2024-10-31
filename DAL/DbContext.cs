using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BLL.Models;

namespace DAL
{
 public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Define your DbSets (tables) here
        public DbSet<Student> Students { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
    }
}
   
