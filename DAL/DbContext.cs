using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ExcelData> ExcelData { get; set; }
    public DbSet<StudentLesson> StudentLessons { get; set; }
    public DbSet<InstructorAvailability> InstructorAvailabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure UserProfile table
        modelBuilder.Entity<UserProfile>()
            .HasKey(up => up.ProfileId);

        modelBuilder.Entity<UserProfile>()
            .HasIndex(up => up.Email)
            .IsUnique();

        // ExcelData -> UserProfiles
        modelBuilder.Entity<ExcelData>()
            .HasKey(ed => ed.ExcelDataId);

        modelBuilder.Entity<ExcelData>()
            .HasOne(ed => ed.UserProfile)
            .WithMany(up => up.ExcelEntries)
            .HasForeignKey(ed => ed.ProfileId);

        // StudentLesson -> UserProfiles
        modelBuilder.Entity<StudentLesson>()
            .HasKey(sl => sl.LessonId);

        modelBuilder.Entity<StudentLesson>()
            .HasOne(sl => sl.UserProfile)
            .WithMany(up => up.Lessons)
            .HasForeignKey(sl => sl.StudentEmail)
            .HasPrincipalKey(up => up.Email);

        modelBuilder.Entity<StudentLesson>()
            .HasOne(sl => sl.UserProfile)
            .WithMany(up => up.Lessons)
            .HasForeignKey(sl => sl.InstructorEmail)
            .HasPrincipalKey(up => up.Email);

        // InstructorAvailability -> UserProfiles
        modelBuilder.Entity<InstructorAvailability>()
            .HasKey(ia => ia.AvailabilityId);

        modelBuilder.Entity<InstructorAvailability>()
            .HasOne(ia => ia.UserProfile)
            .WithMany(up => up.Availabilities)
            .HasForeignKey(ia => ia.InstructorEmail)
            .HasPrincipalKey(up => up.Email);
    }
}
