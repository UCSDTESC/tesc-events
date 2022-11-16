using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services; 

public class UserService : UnitOfWorkBase, IUserService {
    public UserService(RepositoryContext context) : base(context) {
    }

    public void CreateUser(User user) {
        RepositoryContext.Users.Add(user);
    }

    public User? GetUserByEmail(string email) {
        return RepositoryContext.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? GetUser(Guid id) {
        return RepositoryContext.Users.Find(id);
    }

    public void UpdateUser(User user) {
        RepositoryContext.Users.Update(user);
    }
}