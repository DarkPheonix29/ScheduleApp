using BLL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    // Define your DbSets (tables) here
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Event> Events { get; set; }
	public DbSet<UserProfile> UserProfiles { get; set; }

}