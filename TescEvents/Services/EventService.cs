using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public class EventService : UnitOfWorkBase, IEventService {
    public EventService(RepositoryContext context) : base(context) { }
    
    public void RegisterUserForBatch(User user, Batch batch) {
        // Check if batch is at capacity
        var usersInBatch = RepositoryContext.Users.Where(u => u.BatchId == batch.Id);
        // Do nothing if batch is at capacity
        if (usersInBatch.Count() == batch.Capacity) return;
        
        // Otherwise, assign user to batch
        user.Batch = batch;
        user.BatchId = batch.Id;

        RepositoryContext.Users.Update(user);
        RepositoryContext.SaveChanges();
    }

    public IQueryable<Batch> GetAvailableBatches() {
        throw new NotImplementedException();
    }

    public void ClearUserReservation(User user, Batch batch) {
        throw new NotImplementedException();
    }

    public Batch? GetBatch(Guid batchId) {
        throw new NotImplementedException();
    }
}