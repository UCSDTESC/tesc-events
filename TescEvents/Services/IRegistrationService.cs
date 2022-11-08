using TescEvents.Models;

namespace TescEvents.Services; 

public interface IRegistrationService {
    void RegisterUserForBatch(User user, Batch batch);
}