using BLL.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{ }

	// DbSets for your entities
	public DbSet<InstructorAvailability> InstructorAvailabilities { get; set; }
	public DbSet<StudentLesson> StudentLessons { get; set; }
	public DbSet<UserProfile> UserProfiles { get; set; }
}
