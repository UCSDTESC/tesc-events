using TescEvents.Entities;
using TescEvents.Models;

namespace TescEvents.Repositories; 

public class EventRegistrationRepository : RepositoryBase<EventRegistration>, IEventRegistrationRepository {
    public EventRegistrationRepository(RepositoryContext context) : base(context) {
    }

    public void RegisterStudentForEvent(Student student, Event e) {
        Create(new EventRegistration {
            Student = student,
            StudentId = student.Id,
            Event = e,
            EventId = e.Id,
            UserStatus = UserStatuses.PENDING,
            IsResumeSanitized = false,
        });
    }
}