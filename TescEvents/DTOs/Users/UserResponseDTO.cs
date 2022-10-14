namespace TescEvents.DTOs.Users; 

public class UserResponseDTO {
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserType { get; set; }
}