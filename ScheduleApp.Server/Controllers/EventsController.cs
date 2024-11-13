using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using BLL.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SheduleApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // GET: api/events
    [HttpGet("instructor/{instructorId}")]
    public async Task<ActionResult<List<Event>>> GetEventsByInstructorId(int instructorId)
    {
        var events = await _eventService.GetEventsByInstructorIdAsync(instructorId);
        return Ok(events);
    }

    [HttpPost("book/{eventId}")]
    public async Task<ActionResult<Event>> BookLesson(int eventId, [FromBody] int studentId)
    {
        var bookedEvent = await _eventService.BookLessonAsync(eventId, studentId);
        if (bookedEvent == null)
        {
            return BadRequest("Event is already booked or not available.");
        }
        return Ok(bookedEvent);
    }

    [HttpGet("check-availability")]
    public async Task<ActionResult<bool>> CheckAvailability([FromQuery] int instructorId, [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var isAvailable = await _eventService.CheckAvailabilityAsync(instructorId, start, end);
        return Ok(isAvailable);
    }
}