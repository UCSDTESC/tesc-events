using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public interface IUserService : IUnitOfWork {
    /// <summary>
    /// Creates a user in the database
    /// </summary>
    /// <param name="user"></param>
    void CreateUser(User user);
    
    /// <summary>
    /// Retrieves a user matching the given email in the database
    /// </summary>
    /// <param name="email"></param>
    /// <returns>The user matching the email, or null if not found</returns>
    User? GetUserByEmail(string email);
    
    /// <summary>
    /// Retrieves a user by uuid
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The user matching the uuid, or null if not found</returns>
    User? GetUser(Guid id);
    
    /// <summary>
    /// Saves a user's updated information in the database
    /// </summary>
    /// <param name="user"></param>
    void UpdateUser(User user);
}