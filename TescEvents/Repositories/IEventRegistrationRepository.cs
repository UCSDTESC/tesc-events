using TescEvents.Models;

namespace TescEvents.Repositories; 

public interface IEventRegistrationRepository : IRepositoryBase<EventRegistration> {
    void RegisterStudentForEvent(Student student, Event e);
}