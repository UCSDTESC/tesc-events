using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TescEvents.Services;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventsController : ControllerBase {
    private readonly IRegistrationService registrationService;
    public EventsController(IRegistrationService registrationService) {
        this.registrationService = registrationService;
    }
    
    [HttpGet(Name = nameof(GetEventDetails))]
    public IActionResult GetEventDetails() {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("/batch/{batchId:guid}/reserve")]
    public IActionResult ReserveTimeslot(Guid batchId) {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("/batch/{batchId:guid}/cancel")]
    public IActionResult CancelReservation(Guid batchId) {
        throw new NotImplementedException();
    }
}