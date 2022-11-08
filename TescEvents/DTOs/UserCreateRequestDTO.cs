using System.ComponentModel.DataAnnotations;

namespace TescEvents.DTOs; 

public class UserCreateRequestDTO {
    public string Pid { get; set; }
    public string Password { get; set; }
    public string First { get; set; }
    public string Last { get; set; } 
    [EmailAddress]
    public string Email { get; set; } 
    [Phone]
    public string? Phone { get; set; } 
}