using TescEvents.Models;

namespace TescEvents.Services; 

public interface IEventService {
    /// <summary>
    /// Registers a user for a batch if it is not full, replacing their current reservation if they have one. If the batch is full, no action is performed.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="batch"></param>
    void RegisterUserForBatch(User user, Batch batch);
    
    /// <summary>
    /// Gets all batches that are not full
    /// </summary>
    /// <returns>A collection of batches that are not full</returns>
    IQueryable<Batch> GetAvailableBatches();
    
    /// <summary>
    /// Clears a user's reservation for a batch. If the user is not registered for a batch, no action is performed.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="batch"></param>
    void ClearUserReservation(User user, Batch batch);
    
    /// <summary>
    /// Finds a batch with a given batch id.
    /// </summary>
    /// <param name="batchId"></param>
    /// <returns>The batch associated with the batchId, or null if not found</returns>
    Batch? GetBatch(Guid batchId);
}