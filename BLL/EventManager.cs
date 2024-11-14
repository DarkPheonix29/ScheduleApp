using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;

namespace BLL.Managers
{
    public class EventManager : IEventManager
    {
        private readonly IEventRepos _eventRepos;

        public EventManager(IEventRepos eventRepos)
        {
            _eventRepos = eventRepos;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _eventRepos.GetAllEventsAsync();
        }

        public async Task<List<Event>> GetEventsByInstructorIdAsync(int instructorId)
        {
            return await _eventRepos.GetEventsByInstructorIdAsync(instructorId);
        }

        public async Task<Event> AddEventAsync(Event newEvent)
        {
            return await _eventRepos.AddEventAsync(newEvent);
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventRepos.GetEventByIdAsync(id);
        }

        public async Task<Event> UpdateEventAsync(Event updatedEvent)
        {
            return await _eventRepos.UpdateEventAsync(updatedEvent);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            return await _eventRepos.DeleteEventAsync(id);
        }

        public async Task<Event> BookLessonAsync(int eventId, int studentId)
        {
            return await _eventRepos.BookLessonAsync(eventId, studentId);
        }

        public async Task<bool> CheckAvailabilityAsync(int instructorId, DateTime start, DateTime end)
        {
            return await _eventRepos.CheckAvailabilityAsync(instructorId, start, end);
        }
    }
}