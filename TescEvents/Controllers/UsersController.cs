using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TescEvents.DTOs;
using TescEvents.Models;
using TescEvents.Repositories;
using TescEvents.Services;
using TescEvents.Utilities;
using static BCrypt.Net.BCrypt;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
[AllowAnonymous]
public class UsersController : ControllerBase {
    private readonly IMapper mapper;
    private readonly IAuthService authService;
    private readonly IUserService userService;

    public UsersController(IMapper mapper, IAuthService authService, IUserService userService) {
        this.mapper = mapper;
        this.authService = authService;
        this.userService = userService;
    }

    [HttpPost("/register")]
    public IActionResult Register(UserCreateRequestDTO userReq) {
        if (!ModelState.IsValid) return BadRequest();
        var user = mapper.Map<User>(userReq);
        var salt = GenerateSalt();

        user.Salt = salt;
        user.PasswordHash = HashPassword(userReq.Password, salt);
        
        userService.CreateUser(user);
        userService.Commit();

        return Ok();
    }

    [HttpPost("/login")]
    public IActionResult Login([FromForm] string email, [FromForm] string password) {
        var user = userService.GetUserByEmail(email);
        if (user == null) return Unauthorized();

        if (HashPassword(password, user.Salt) != user.PasswordHash) return Unauthorized();

        var jwt = authService.CreateJwt(user.Pid, user.Email);
        return Ok(jwt);
    }
}