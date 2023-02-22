using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public class EventService : IEventService {
    public EventService(RepositoryContext context) : base() { }

    public IQueryable<Event> GetFutureEvents() {
        //throw new NotImplementedException();
        DateTime timeNow = DateTime.Now;
        var futureEvents = RepositoryContext.Events.Where(u => u.Start >= timeNow);
        return futureEvents;
    }

    public Event? GetEventDetails(Guid eventId) {
        //throw new NotImplementedException();
        var event = RepositoryContext.Events!.FirstOrDefault(e => e.Id == eventId);
        return event;
    }
    
}