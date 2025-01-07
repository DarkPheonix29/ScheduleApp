using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class StudentLesson
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	public string InstructorEmail { get; set; }

	[Required]
	public string StudentEmail { get; set; }

	[Required]
	public DateTime Start { get; set; }

	[Required]
	public DateTime End { get; set; }

	public string Status { get; set; } // "Booked", "Cancelled"
}
