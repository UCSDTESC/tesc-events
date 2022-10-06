using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;

namespace TescEvents.Repositories; 

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class {
    protected RepositoryContext RepositoryContext { get; set; }

    public RepositoryBase(RepositoryContext context) {
        this.RepositoryContext = context;
    }

    public IQueryable<T> FindAll() {
        return this.RepositoryContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) {
        return this.RepositoryContext.Set<T>()
                   .Where(expression).AsNoTracking();
    }

    public void Create(T entity) {
        this.RepositoryContext.Set<T>().Add(entity);
    }

    public void Update(T entity) {
        this.RepositoryContext.Set<T>().Update(entity);
    }

    public void Delete(T entity) {
        this.RepositoryContext.Set<T>().Remove(entity);
    }

    public void Save() {
        this.RepositoryContext.SaveChanges();
    }
}