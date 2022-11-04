using Microsoft.AspNetCore.Mvc;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase {
    public UsersController() {
        
    }

    [HttpPost("/register")]
    public IActionResult Register() {
        throw new NotImplementedException();
    }

    [HttpPost("/login")]
    public IActionResult Login() {
        throw new NotImplementedException();
    }
}