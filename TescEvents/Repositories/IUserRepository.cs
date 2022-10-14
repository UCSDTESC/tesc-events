using TescEvents.Models;

namespace TescEvents.Repositories; 

public interface IUserRepository : IRepositoryBase<User> {
    /// <summary>
    /// Returns the user entity matching the username, or null if not found
    /// </summary>
    /// <param name="username">Username of user</param>
    /// <returns>User matching username, or null if not found</returns>
    User? GetUserByUsername(string username);
}