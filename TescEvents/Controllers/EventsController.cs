using Microsoft.AspNetCore.Mvc;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventsController : ControllerBase {
    public EventsController() {
    }
    
    [HttpGet(Name = nameof(GetEventDetails))]
    public IActionResult GetEventDetails() {
        throw new NotImplementedException();
    }

    [HttpPost("/batch/{batchId:guid}/reserve")]
    public IActionResult ReserveTimeslot(Guid batchId) {
        throw new NotImplementedException();
    }

    [HttpPost("/batch/{batchId:guid}/cancel")]
    public IActionResult CancelReservation(Guid batchId) {
        throw new NotImplementedException();
    }
}