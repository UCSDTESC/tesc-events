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
    private readonly IEmailService emailService;

    public EventController(IEventService eventService, IUserService userService, IEmailService emailService) {
        this.eventService = eventService;
        this.userService = userService;
        this.emailService = emailService;
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

        var isReservationUpdate = user.Batch != null;
        eventService.RegisterUserForBatch(user, batch);
        if (isReservationUpdate) {
            emailService.SendReservationUpdateEmail(user.Email, batch);
        } else {
            emailService.SendReservationConfirmationEmail(user.Email, batch);
        }
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
        emailService.SendReservationCancellationEmail(user.Email, batch);
        return Ok();
    }
}