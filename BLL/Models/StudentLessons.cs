using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class StudentLesson
{
	public int LessonId { get; set; } // Primary Key
	public string InstructorEmail { get; set; } // Foreign Key (linked to UserProfiles.Email)
	public string StudentEmail { get; set; } // Foreign Key (linked to UserProfiles.Email)
	public DateTime Start { get; set; }
	public DateTime End { get; set; }
	public string Status { get; set; } // "Booked", "Cancelled"

	// Navigation Properties
	public UserProfile UserProfile { get; set; }
}

