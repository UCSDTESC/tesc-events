using TescEvents.Models;

namespace TescEvents.Repositories; 

public interface IUserRepository : IRepositoryBase<User> {
    /// <summary>
    /// Returns the user entity matching the username, or null if not found
    /// </summary>
    /// <param name="username">Username of user</param>
    /// <returns>User matching username, or null if not found</returns>
    User? GetUserByUsername(string username);

    /// <summary>
    /// Returns the user entity matching the uuid, or null if not found
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns>User matching uuid, or null if not found</returns>
    User? GetUserByUuid(string uuid);

    /// <summary>
    /// Inserts a User entity in the database
    /// <param name="user">User to be created</param>
    /// </summary>
    void CreateUser(User user);
}