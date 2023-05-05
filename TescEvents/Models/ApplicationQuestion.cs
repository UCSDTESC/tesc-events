using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TescEvents.Models; 

[Table("ApplicationQuestions")]
public class ApplicationQuestion {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    public Guid EventApplicationId { get; set; }

    public string? Question { get; set; }

    public string? Description { get; set; } 
    
}