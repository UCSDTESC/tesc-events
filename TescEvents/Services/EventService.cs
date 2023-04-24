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

    public void RegisterUserForEvent(Guid eventId, Guid studentId) {
        var registration = new EventRegistration {
            EventId = eventId,
            StudentId = studentId,
            UserStatus = UserStatus.Pending,
            IsResumeSanitized = false,
        };
        context.EventRegistrations!.Add(registration);
        context.SaveChanges();
    }

    public EventRegistration? GetEventRegistration(Guid registrationId) {
        return context.EventRegistrations!.FirstOrDefault(e => e.Id == registrationId);
    }

    public EventRegistration? GetEventRegistration(Guid eventId, Guid studentId) {
        return context.EventRegistrations!
                      .FirstOrDefault(e => e.EventId == eventId && e.StudentId == studentId);
    }
}