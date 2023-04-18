using Microsoft.AspNetCore.Mvc;
using TescEvents.Models;
using TescEvents.Services;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventsController : ControllerBase {
    private readonly IEventService eventService;

    public EventsController(IEventService eventService) {
        this.eventService = eventService;
    }
    
    [HttpGet(Name = nameof(GetEvents))]
    public IEnumerable<Event> GetEvents(string? start = "", string? end = "") {
        if (!DateTime.TryParse(start, out var startFilter)) {
            startFilter = DateTime.UnixEpoch;
        }

        if (!DateTime.TryParse(end, out var endFilter)) {
            endFilter = DateTime.Now;
        }

        return eventService.GetFutureEvents();
    }
}