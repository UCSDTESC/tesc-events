using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TescEvents.Models; 

[Table("Users")]
public class User {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public string Pid { get; set; }
    
    [MaxLength(255)]
    public string First { get; set; }
    
    [MaxLength(255)]
    public string Last { get; set; }
    
    [MaxLength(255)]
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string Salt { get; set; }
    
    [MaxLength(255)]
    public string Phone { get; set; }
    
    [ForeignKey(nameof(Batch))]
    public Guid? BatchId { get; set; }
    
    public Batch? Batch { get; set; }
}