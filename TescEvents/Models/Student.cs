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
    [MaxLength(255)]
    public string Username { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string LastName { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [Required]
    public string Salt { get; set; }

    [Required]
    [MaxLength(255)]
    public string UserType { get; set; } = UserTypes.REGULAR;
    
    [MaxLength(255)]
    public string Year { get; set; }
    
    [MaxLength(255)]
    public string University { get; set; }
    
    [MaxLength(255)]
    public string Phone { get; set; }
    
    [MaxLength(255)]
    public string GPA { get; set; }
    
    [MaxLength(255)]
    public string PID { get; set; }
    
    [MaxLength(255)]
    public string Gender { get; set; }
    
    [MaxLength(255)]
    public string Pronouns { get; set; }
    
    [MaxLength(255)]
    public string Ethnicity { get; set; }
}

public class UserTypes {
    public const string REGULAR = "REGULAR";
    public const string ADMIN = "ADMIN";
    public const string ORGANIZER = "ORGANIZER";
}