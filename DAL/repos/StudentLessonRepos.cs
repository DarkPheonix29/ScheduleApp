using BLL.Interfaces;
using System.Data.Entity;

public class StudentLessonRepos : IStudentLessonRepos
{
	private readonly ApplicationDbContext _context;

	public StudentLessonRepos(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<StudentLesson> BookLessonAsync(StudentLesson lesson)
	{
		var availabilitySlot = await _context.InstructorAvailabilities
											 .FirstOrDefaultAsync(a => a.Start == lesson.Start && a.Status == "Available");

		if (availabilitySlot != null)
		{
			availabilitySlot.Status = "Booked"; // Mark the availability slot as booked.
			_context.StudentLessons.Add(lesson);
			await _context.SaveChangesAsync();
			return lesson;
		}

		return null; // Slot is not available.
	}

	public async Task<List<StudentLesson>> GetLessonsByInstructorEmailAsync(string instructorEmail)
	{
		return await _context.StudentLessons
							 .Where(l => l.InstructorEmail == instructorEmail)
							 .ToListAsync();
	}
}
