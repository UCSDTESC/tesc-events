using TescEvents.Entities;
using TescEvents.Models;

namespace TescEvents.Repositories; 

public class UserRepository : RepositoryBase<User>, IUserRepository {
    public UserRepository(RepositoryContext context) : base(context) {
    }

    public User? GetUserByUsername(string username) {
        return FindByCondition(user => user.Username == username)
            .FirstOrDefault();
    }

    public User? GetUserByUuid(Guid uuid) {
        return FindByCondition(user => user.Id == uuid)
            .FirstOrDefault();
    }

    public void CreateUser(User user) {
        Create(user);
        Save();
    }

}