using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TescEvents.Models; 

[Table("Batches")]
public class Batch {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    public DateTime Timeslot { get; set; }
    
    public TimeSpan Length { get; set; }
    
    public int Capacity { get; set; }
    
    public int WaitlistCapacity { get; set; }
}