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
    [HttpGet]
    public async Task<ActionResult<List<Event>>> GetEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    // POST: api/events
    [HttpPost]
    public async Task<ActionResult<Event>> AddEvent([FromBody] Event newEvent)
    {
        var createdEvent = await _eventService.AddEventAsync(newEvent);
        return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
    }

    // GET: api/events/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        var eventItem = await _eventService.GetEventByIdAsync(id);
        if (eventItem == null)
        {
            return NotFound();
        }
        return Ok(eventItem);
    }

    // PUT: api/events/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
    {
        if (id != updatedEvent.Id)
        {
            return BadRequest();
        }

        var result = await _eventService.UpdateEventAsync(updatedEvent);
        if (result == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/events/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var success = await _eventService.DeleteEventAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}