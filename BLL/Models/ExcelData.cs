using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ExcelData
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int ExcelDataId { get; set; }

	[Required]
	public int ProfileId { get; set; } // Foreign key to UserProfiles

	[Required]
	public string Category { get; set; }

	[Required]
	public string Topic { get; set; }

	[Required]
	public string Subtopic { get; set; }

	

	// Columns for each lesson (adjust based on your actual number of lessons)
	public string? Les1 { get; set; }
	public string? Les2 { get; set; }
	public string? Les3 { get; set; }
	public string? Les4 { get; set; }
	public string? Les5 { get; set; }
	public string? Les6 { get; set; }
	public string? Les7 { get; set; }
	public string? Les8 { get; set; }
	public string? Les9 { get; set; }
	public string? Les10 { get; set; }
	public string? Les11 { get; set; }
	public string? Les12 { get; set; }
	public string? Les13 { get; set; }
	public string? Les14 { get; set; }
	public string? Les15 { get; set; }
	public string? Les16 { get; set; }
	public string? Les17 { get; set; }
	public string? Les18 { get; set; }
	public string? Les19 { get; set; }
	public string? Les20 { get; set; }
	public string? Les21 { get; set; }
	public string? Les22 { get; set; }
	public string? Les23 { get; set; }
	public string? Les24 { get; set; }
	public string? Les25 { get; set; }
	public string? Les26 { get; set; }
	public string? Les27 { get; set; }
	public string? Les28 { get; set; }
	public string? Les29 { get; set; }
	public string? Les30 { get; set; }
	public string? Les31 { get; set; }
	public string? Les32 { get; set; }
	public string? Les33 { get; set; }
	public string? Les34 { get; set; }
	public string? Les35 { get; set; }
	public string? Les36 { get; set; }
	public string? Les37 { get; set; }
	public string? Les38 { get; set; }
	public string? Les39 { get; set; }
	public string? Les40 { get; set; }
	public string? Les41 { get; set; }
	public string? Les42 { get; set; }
	public string? Les43 { get; set; }
	public string? Les44 { get; set; }
	public string? Les45 { get; set; }
	public string? Les46 { get; set; }
	public string? Les47 { get; set; }
	public string? Les48 { get; set; }
	public string? Les49 { get; set; }
	public string? Les50 { get; set; }

	// Navigation property
	public UserProfile UserProfile { get; set; }
}