using Microsoft.AspNetCore.Mvc;
using TescEvents.Models;
using TescEvents.Services;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventsController : ControllerBase {
    private readonly IEventService eventService;
    private readonly IAuthService authService;

    public EventsController(IEventService eventService, IAuthService authService) {
        this.eventService = eventService;
        this.authService = authService;
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

    [HttpPost("{eventId:guid}/register")]
    public IActionResult RegisterUserForEvent(Guid eventId, string jwt) {
        Guid? userId = authService.ValidateJwt(jwt);
        if (userId == null) return Forbid();

        if (eventService.GetEventDetails(eventId) == null) return NotFound();

        if (eventService.GetEventRegistration(eventId, userId.Value) != null) return BadRequest();

        eventService.RegisterUserForEvent(eventId, userId.Value);
        
        return Ok();
    }
}