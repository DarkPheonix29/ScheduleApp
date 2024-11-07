using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services;

public class EventService : IEventService  
{
    private readonly ApplicationDbContext _context;

    // Constructor should be defined separately
    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all events
    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    // Add a new event
    public async Task<Event> AddEventAsync(Event newEvent)
    {
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return newEvent;
    }

    // Get event by Id
    public async Task<Event> GetEventByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    // Update an existing event
    public async Task<Event> UpdateEventAsync(Event updatedEvent)
    {
        var existingEvent = await _context.Events.FindAsync(updatedEvent.Id);
        if (existingEvent != null)
        {
            existingEvent.Title = updatedEvent.Title;
            existingEvent.Start = updatedEvent.Start;
            existingEvent.End = updatedEvent.End;
            await _context.SaveChangesAsync();
            return existingEvent;
        }
        return null;
    }

    // Delete an event
    public async Task<bool> DeleteEventAsync(int id)
    {
        var eventToDelete = await _context.Events.FindAsync(id);
        if (eventToDelete != null)
        {
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}