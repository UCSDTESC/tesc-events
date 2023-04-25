using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

    [HttpPost("event/{eventId:guid}/register"), Authorize]
    public IActionResult RegisterUserForEvent(Guid eventId) {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return Forbid();

        var actor = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor);
        if (actor == null || actor.Value.IsNullOrEmpty()) return Forbid();

        Guid? userId = Guid.Parse(actor.Value);
        
        if (eventService.GetEventDetails(eventId) == null) return NotFound();

        if (eventService.GetEventRegistration(eventId, userId.Value) != null) return BadRequest();

        eventService.RegisterUserForEvent(eventId, userId.Value);
        
        return Ok();
    }
}