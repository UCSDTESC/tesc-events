using System.Linq.Expressions;

namespace TescEvents.Repositories; 

public interface IRepositoryBase<T> {
    IQueryable<T> FindAll();
    /// <summary>
    /// Returns the model in the database context matching the specified predicate, or null if not found
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Save();
}