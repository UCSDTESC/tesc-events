using TescEvents.Models;

namespace TescEvents.Services; 

public interface IAuthService {
    Student? GetStudentFromClaim(string jwt);
}