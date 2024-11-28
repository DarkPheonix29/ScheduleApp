using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Manager
{
    public interface IEventManager
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<List<Event>> GetEventsByInstructorIdAsync(int instructorId);
        Task<Event> AddEventAsync(Event newEvent);
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> UpdateEventAsync(Event updatedEvent);
        Task<bool> DeleteEventAsync(int id);
        Task<Event> BookLessonAsync(int eventId, int studentId);
        Task<bool> CheckAvailabilityAsync(int instructorId, DateTime start, DateTime end);
    }
}