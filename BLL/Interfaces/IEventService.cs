using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces;

public interface IEventService
{
    Task<List<Event>> GetAllEventsAsync();
    Task<Event> AddEventAsync(Event newEvent);
    Task<Event> GetEventByIdAsync(int id);
    Task<Event> UpdateEventAsync(Event updatedEvent);
    Task<bool> DeleteEventAsync(int id);
}