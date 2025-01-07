using System;

namespace BLL.Models;

// AvailabilityRequest.cs
public class AvailabilityRequest
{
	public string InstructorEmail { get; set; }
	public DateTime Start { get; set; }
	public DateTime End { get; set; }
	public int Duration { get; set; } // Duration in hours
}

