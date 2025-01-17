using BLL.Manager;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InstructorAvailabilityController : ControllerBase
{
	private readonly IEventManager _availabilityManager;

	public InstructorAvailabilityController(IEventManager availabilityManager)
	{
		_availabilityManager = availabilityManager;
	}

	[HttpPost("add-availability")]
	public async Task<IActionResult> AddAvailability([FromBody] AddAvailability availability)
	{
		if (string.IsNullOrWhiteSpace(availability.InstructorEmail))
		{
			return BadRequest(new { message = "Instructor email is required" });
		}

		try
		{
			InstructorAvailability instructorAvailability = await _availabilityManager.AddAvailabilityAsync(availability.InstructorEmail, availability.Start, availability.End, availability.Status);
			return Ok(new { message = "Availability added successfully." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpGet("all-availability")]
	public async Task<IActionResult> GetAllAvailability()
	{
		var availabilities = await _availabilityManager.GetInstructorAvailabilityAsync();
		return Ok(availabilities);
	}
}

public class AddAvailability
{
	public string InstructorEmail { get; set; }
	public DateTime Start { get; set; }
	public DateTime End { get; set; }
	public string Status { get; set; }
}
