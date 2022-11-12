using TescEvents.Entities;
using TescEvents.Models;

namespace TescEvents.Repositories; 

public class EventRegistrationRepository : RepositoryBase<EventRegistration>, IEventRegistrationRepository {
    public EventRegistrationRepository(RepositoryContext context) : base(context) {
    }

    public void RegisterStudentForEvent(Student student, Event e) {
        Create(new EventRegistration {
            StudentId = student.Id,
            EventId = e.Id,
            UserStatus = UserStatuses.PENDING,
            IsResumeSanitized = false,
        });
        Save();
    }
}