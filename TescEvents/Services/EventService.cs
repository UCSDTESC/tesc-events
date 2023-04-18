using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public class EventService : IEventService {

    private RepositoryContext context;
    public EventService(RepositoryContext context) {
        this.context = context;
    }

    public IEnumerable<Event> GetFutureEvents() {
        //throw new NotImplementedException();
        DateTime timeNow = DateTime.UtcNow;
        var futureEvents = context.Events!.Where(u => u.Start.ToUniversalTime() >= timeNow);
        return futureEvents;
    }

    public Event? GetEventDetails(Guid eventId) {
        var currEvent = context.Events!.FirstOrDefault(e => e.Id == eventId);
        return currEvent;
    }
}