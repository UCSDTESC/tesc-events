using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TescEvents.Services;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventController : ControllerBase {
    private readonly IEventService eventService;
    private readonly IUserService userService;

    public EventController(IEventService eventService, IUserService userService) {
        this.eventService = eventService;
        this.userService = userService;
    }
    
    [HttpGet(Name = nameof(GetEventDetails))]
    public IActionResult GetEventDetails() {
        var availableTimes = eventService.GetAvailableBatches().ToList();
        return Ok(availableTimes);
    }

    [Authorize]
    [HttpPost("/{batchId:guid}/reserve")]
    public IActionResult ReserveTimeslot(Guid batchId) {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        if (userId == null) return Unauthorized();

        var user = userService.GetUser(Guid.Parse(userId));
        if (user == null) return Unauthorized();

        var batch = eventService.GetBatch(batchId);
        if (batch == null) return NotFound();
        
        eventService.RegisterUserForBatch(user, batch);
        return NoContent();
    }

    [Authorize]
    [HttpPost("/{batchId:guid}/cancel")]
    public IActionResult CancelReservation(Guid batchId) {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        if (userId == null) return Unauthorized();

        var user = userService.GetUser(Guid.Parse(userId));
        if (user == null) return Unauthorized();

        var batch = eventService.GetBatch(batchId);
        if (batch == null) return NotFound();
        
        eventService.ClearUserReservation(user, batch);
        return Ok();
    }
}