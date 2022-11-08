using TescEvents.Entities;

namespace TescEvents.Repositories;

public abstract class UnitOfWorkBase : IUnitOfWork {
    protected RepositoryContext RepositoryContext { get; }

    protected UnitOfWorkBase(RepositoryContext context) {
        RepositoryContext = context;
    }
    
    public void Commit() {
        RepositoryContext.SaveChanges();
    }
}