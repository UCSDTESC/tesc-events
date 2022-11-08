using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public interface IUserService : IUnitOfWork {
    void CreateUser(User user);
    User? GetUserByEmail(string email);
    User? GetUser(Guid id);
    void UpdateUser(User user);
}