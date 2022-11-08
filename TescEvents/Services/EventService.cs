using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public class EventService : UnitOfWorkBase, IEventService {
    public EventService(RepositoryContext context) : base(context) { }
    
    public void RegisterUserForBatch(User user, Batch batch) {
        throw new NotImplementedException();
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