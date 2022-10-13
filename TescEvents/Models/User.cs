using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TescEvents.Models; 

[Table("Users")]
public class User {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [Required]
    public string Salt { get; set; }

    [Required]
    public string UserType { get; set; } = UserTypes.REGULAR;
}

public class UserTypes {
    public const string REGULAR = "REGULAR";
    public const string ADMIN = "ADMIN";
    public const string COORDINATOR = "COORDINATOR";
}