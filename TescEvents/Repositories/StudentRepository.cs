using TescEvents.Entities;
using TescEvents.Models;

namespace TescEvents.Repositories; 

public class StudentRepository : RepositoryBase<Student>, IStudentRepository {
    public StudentRepository(RepositoryContext context) : base(context) {
    }

    public Student? GetUserByUsername(string username) {
        return FindByCondition(user => user.Username == username)
            .FirstOrDefault();
    }

    public Student? GetUserByUuid(Guid uuid) {
        return FindByCondition(user => user.Id == uuid)
            .FirstOrDefault();
    }

    public void CreateUser(Student student) {
        Create(student);
        Save();
    }

}