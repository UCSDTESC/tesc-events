using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TescEvents.Models; 

[Table("Students")]
[Index(nameof(Username), IsUnique = true)]
public class Student {
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
    
    public string Year { get; set; }
    
    public string University { get; set; }
    
    public string Phone { get; set; }
    
    public string GPA { get; set; }
    
    public string PID { get; set; }
    
    public string Gender { get; set; }
    
    public string Pronouns { get; set; }
    
    public string Ethnicity { get; set; }
}

public class UserTypes {
    public const string REGULAR = "REGULAR";
    public const string ADMIN = "ADMIN";
    public const string ORGANIZER = "ORGANIZER";
}