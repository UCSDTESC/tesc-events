using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public interface IUserService {
    /// <summary>
    /// Creates a user in the database
    /// </summary>
    /// <param name="student"></param>
    void CreateStudent(Student student);
    
    /// <summary>
    /// Retrieves a user matching the given email in the database
    /// </summary>
    /// <param name="email"></param>
    /// <returns>The user matching the email, or null if not found</returns>
    Student? GetStudentByEmail(string email);
    
    /// <summary>
    /// Retrieves a user by uuid
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The user matching the uuid, or null if not found</returns>
    Student? GetStudent(Guid id);
    
    /// <summary>
    /// Saves a user's updated information in the database
    /// </summary>
    /// <param name="student"></param>
    void UpdateStudent(Student student);
}