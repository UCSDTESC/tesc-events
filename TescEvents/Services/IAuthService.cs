namespace TescEvents.Services; 

public interface IAuthService {
    string CreateJwt(string pid, string email);
}