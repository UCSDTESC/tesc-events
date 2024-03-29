using TescEvents.Models;

namespace TescEvents.Services; 

// TODO: include start and end dates 
public interface IEventService {

    /// <summary>
    /// Gets all future events
    /// </summary>
    /// <returns>A collection of future events</returns>
    IEnumerable<Event>? GetFutureEvents();

    /// <summary>
    /// Finds an event with the given event ID
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns>The event associated with the eventId, or null if not found</returns>
    Event? GetEventDetails(Guid eventId);

}