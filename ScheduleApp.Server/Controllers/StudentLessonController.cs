using BLL.Manager;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StudentLessonController : ControllerBase
{
	private readonly IEventManager _lessonManager;

	public StudentLessonController(IEventManager lessonManager)
	{
		_lessonManager = lessonManager;
	}

	[HttpPost("book-lesson")]
	public async Task<IActionResult> BookLesson([FromBody] StudentLesson lesson)
	{
		var bookedLesson = await _lessonManager.BookLessonAsync(lesson);
		if (bookedLesson != null)
		{
			return Ok(bookedLesson);
		}

		return BadRequest("Slot not available.");
	}

	[HttpGet("all-availability")]
	public async Task<IActionResult> GetAllAvailability()
	{
		var availabilities = await _lessonManager.GetInstructorAvailabilityAsync();
		return Ok(availabilities);
	}

}
