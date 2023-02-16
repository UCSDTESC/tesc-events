namespace TescEvents.Services; 

public interface IAuthService {
    string CreateJwt(Guid userId, string email);
}