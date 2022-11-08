using TescEvents.Models;

namespace TescEvents.Services; 

public interface IEventService {
    void RegisterUserForBatch(User user, Batch batch);
    IQueryable<Batch> GetAvailableBatches();
    void ClearUserReservation(User user, Batch batch);
    Batch? GetBatch(Guid batchId);
}