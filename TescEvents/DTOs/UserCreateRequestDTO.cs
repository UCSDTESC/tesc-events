using System.ComponentModel.DataAnnotations;

namespace TescEvents.DTOs; 

public class UserCreateRequestDTO {
    public string Pid { get; set; }
    public string Password { get; set; }
    public string First { get; set; }
    public string Last { get; set; } 
    public string Email { get; set; } 
    public string? Phone { get; set; }
    public IFormFile? Resume { get; set; }
    public string? BatchId { get; set; }
}