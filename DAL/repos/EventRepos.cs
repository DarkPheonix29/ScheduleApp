﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos;

public class EventRepos : IEventRepos
{
    private readonly ApplicationDbContext _context;

    public EventRepos(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<List<Event>> GetEventsByInstructorIdAsync(int instructorId)
    {
        return await _context.Events.Where(e => e.InstructorId == instructorId).ToListAsync();
    }

    public async Task<Event> AddEventAsync(Event newEvent)
    {
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return newEvent;
    }

    public async Task<Event> GetEventByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<Event> UpdateEventAsync(Event updatedEvent)
    {
        var existingEvent = await _context.Events.FindAsync(updatedEvent.Id);
        if (existingEvent != null)
        {
            existingEvent.Title = updatedEvent.Title;
            existingEvent.Start = updatedEvent.Start;
            existingEvent.End = updatedEvent.End;
            existingEvent.Status = updatedEvent.Status;
            existingEvent.StudentId = updatedEvent.StudentId;
            await _context.SaveChangesAsync();
            return existingEvent;
        }
        return null;
    }

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

	public async Task<Event> BookLessonAsync(int eventId, int studentId)
	{
		var eventToBook = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId && e.Status == "Available");
		if (eventToBook != null)
		{
			eventToBook.Status = "Booked";
			eventToBook.StudentId = studentId;  // Assuming you're assigning the studentId
			await _context.SaveChangesAsync();
		}
		return eventToBook;
	}


	public async Task<bool> CheckAvailabilityAsync(int instructorId, DateTime start, DateTime end)
	{
		var overlappingEvents = await _context.Events
			.Where(e => e.InstructorId == instructorId && e.Status == "Available" &&
						((e.Start < end && e.End > start)))  // Check if times overlap
			.ToListAsync();

		return !overlappingEvents.Any();  // Return true if no overlapping events are found
	}
}
