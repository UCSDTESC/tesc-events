using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services; 

public class UserService : UnitOfWorkBase, IUserService {
    public UserService(RepositoryContext context) : base(context) {
    }

    public void CreateUser(User user) {
        throw new NotImplementedException();
    }

    public User? GetUserByEmail(string email) {
        throw new NotImplementedException();
    }
}