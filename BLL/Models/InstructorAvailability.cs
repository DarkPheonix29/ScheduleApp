using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class InstructorAvailability
{
	public int AvailabilityId { get; set; } // Primary Key
	public string InstructorEmail { get; set; } // Foreign Key (linked to UserProfiles.Email)
	public DateTime Start { get; set; }
	public DateTime End { get; set; }
	public string Status { get; set; } // "Available" or "Booked"

	// Navigation Property
	public UserProfile UserProfile { get; set; }
}

