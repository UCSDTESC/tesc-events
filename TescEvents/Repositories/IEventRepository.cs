using TescEvents.Models;

namespace TescEvents.Repositories; 

public interface IEventRepository : IRepositoryBase<Event> {
    /// <summary>
    /// Gets an event by UUID, or null if not found
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Event? GetEventByUuid(Guid eventId);

    /// <summary>
    /// Returns all events with a start time between start and end
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    IEnumerable<Event> GetAllEventsWithinRange(DateTime start, DateTime end);
}