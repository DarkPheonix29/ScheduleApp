using Microsoft.AspNetCore.Mvc;
using BLL.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using BLL.Manager;

namespace ScheduleApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventManager _eventManager;

        public EventsController(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        // GET: api/events/instructor/{instructorId}
        [HttpGet("instructor/{instructorId}")]
        public async Task<ActionResult<List<Event>>> GetEventsByInstructorId(int instructorId)
        {
            var events = await _eventManager.GetEventsByInstructorIdAsync(instructorId);
            return Ok(events);
        }

        // POST: api/events/book/{eventId}
        [HttpPost("book/{eventId}")]
        public async Task<ActionResult<Event>> BookLesson(int eventId, [FromBody] int studentId)
        {
            var bookedEvent = await _eventManager.BookLessonAsync(eventId, studentId);
            if (bookedEvent == null)
            {
                return BadRequest("Event is already booked or not available.");
            }
            return Ok(bookedEvent);
        }

        // GET: api/events/check-availability
        [HttpGet("check-availability")]
        public async Task<ActionResult<bool>> CheckAvailability([FromQuery] int instructorId, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var isAvailable = await _eventManager.CheckAvailabilityAsync(instructorId, start, end);
            return Ok(isAvailable);
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<Event>> AddEvent([FromBody] Event newEvent)
        {
            var createdEvent = await _eventManager.AddEventAsync(newEvent);
            if (createdEvent == null)
            {
                return BadRequest("Could not create the event.");
            }
            return Ok(createdEvent);
        }
    }
}
