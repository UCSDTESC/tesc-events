using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public class RegistrationService : UnitOfWorkBase, IRegistrationService {
    public RegistrationService(RepositoryContext context) : base(context) { }
    
    public void RegisterUserForBatch(User user, Batch batch) {
        throw new NotImplementedException();
    }
}