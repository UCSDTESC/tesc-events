namespace TescEvents.DTOs.Users; 

public class UserCreateRequestDTO {
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
}