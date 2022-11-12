namespace TescEvents.DTOs.Events;

public class EventPublicResponseDTO {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }
    public string Cover { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}