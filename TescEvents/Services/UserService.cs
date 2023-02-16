using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services; 

public class UserService : IUserService {
    private RepositoryContext context;
    public UserService(RepositoryContext context) {
        this.context = context;
    }

    public void CreateStudent(Student user) {
        context.Students.Add(user);
        context.SaveChanges();
    }

    public Student? GetStudentByEmail(string email) {
        return context.Students.FirstOrDefault(u => u.Email == email);
    }

    public Student? GetStudent(Guid id) {
        return context.Students.Find(id);
    }

    public void UpdateStudent(Student user) {
        context.Students.Update(user);
        context.SaveChanges();
    }
}