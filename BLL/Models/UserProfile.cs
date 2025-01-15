using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class UserProfile
{
	public int ProfileId { get; set; } // Primary Key
	public string Email { get; set; } // Unique
	public string DisplayName { get; set; }
	public string PhoneNumber { get; set; }
	public string Address { get; set; }
	public string PickupAddress { get; set; }
	public DateTime DateOfBirth { get; set; }

	// Navigation Properties
	public ICollection<InstructorAvailability> Availabilities { get; set; } // For instructors
	public ICollection<StudentLesson> Lessons { get; set; } // For students
	public ICollection<ExcelData> ExcelEntries { get; set; }
}


	// Navigation properties
	
	
