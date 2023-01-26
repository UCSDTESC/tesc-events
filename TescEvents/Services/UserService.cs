using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services; 

public class UserService : IUserService {
    public UserService(RepositoryContext context) : base(context) {
    }

    public void CreateStudent(Student user) {
        RepositoryContext.Students.Add(user);
    }

    public Student? GetStudentByEmail(string email) {
        return RepositoryContext.Students.FirstOrDefault(u => u.Email == email);
    }

    public Student? GetStudent(Guid id) {
        return RepositoryContext.Students.Find(id);
    }

    public void UpdateStudent(Student user) {
        RepositoryContext.Students.Update(user);
    }
}