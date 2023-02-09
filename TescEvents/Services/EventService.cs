using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public class EventService : IEventService {
    public EventService(RepositoryContext context) : base() { }

    public IQueryable<Event> GetFutureEvents() {
        throw new NotImplementedException();
    }

    public Event? GetEventDetails(Guid eventId) {
        throw new NotImplementedException();
    }
    
}