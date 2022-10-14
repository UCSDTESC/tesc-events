using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TescEvents.DTOs.Users;
using TescEvents.Repositories;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase {
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    
    public UsersController(IUserRepository userRepository, IMapper mapper) {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }
    
    [AllowAnonymous]
    [HttpGet(Name = nameof(GetUser))]
    [Route("user/{uuid}")]
    public async Task<IActionResult> GetUser(string uuid) {
        var user = userRepository.GetUserByUuid(Guid.Parse(uuid));
        if (user == null) return NotFound();

        var userResponse = mapper.Map<UserResponseDTO>(user);
        return Ok(userResponse);
    } 
}