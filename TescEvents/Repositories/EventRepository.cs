using TescEvents.Entities;
using TescEvents.Models;

namespace TescEvents.Repositories; 

public class EventRepository : RepositoryBase<Event>, IEventRepository {
    public EventRepository(RepositoryContext context) : base(context) {
    }

    public Event? GetEventByUuid(Guid eventId) {
        return FindByCondition(e => e.Id == eventId).FirstOrDefault();
    }
}