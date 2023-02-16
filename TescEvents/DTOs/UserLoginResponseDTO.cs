namespace TescEvents.DTOs; 

public class UserLoginResponseDTO {
    public UserLoginResponseDTO(Guid userId, string jwt) {
        this.Id = userId;
        this.Jwt = jwt;
    }

    public Guid Id { get; set; }
    public string Jwt { get; set; }
}