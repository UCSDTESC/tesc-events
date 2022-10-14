using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TescEvents.DTOs;
using TescEvents.DTOs.Users;
using TescEvents.Models;
using TescEvents.Repositories;
using TescEvents.Utilities;
using static BCrypt.Net.BCrypt;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IUserRepository userRepository;
    private readonly IValidator<User> userValidator;
    private readonly IMapper mapper;
    
    public AuthController(IUserRepository userRepository, IMapper mapper, IValidator<User> userValidator) {
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.userValidator = userValidator;
    }

    [AllowAnonymous]
    [HttpPost(Name = nameof(RegisterUser))]
    public async Task<IActionResult> RegisterUser([Required] [FromForm] UserCreateRequestDTO userReq) {
        var userEntity = mapper.Map<User>(userReq);
        
        var validationResult = await userValidator.ValidateAsync(userEntity);
        if (!validationResult.IsValid) return BadRequest(
                                                         validationResult.Errors
                                                                         .Select(error => error.ErrorMessage));

        var salt = GenerateSalt();
        userEntity.Salt = salt;
        userEntity.PasswordHash = HashPassword(userReq.Password, salt);
        userRepository.CreateUser(userEntity);

        var userResponse = mapper.Map<UserResponseDTO>(userEntity);
        return CreatedAtRoute(nameof(GetUser), new { Id = userResponse.Id }, userResponse);
    }

    [AllowAnonymous]
    [HttpGet("/user/{uuid}", Name = nameof(GetUser))]
    public async Task<IActionResult> GetUser(string uuid) {
        var user = userRepository.GetUserByUuid(uuid);
        if (user == null) return NotFound();

        var userResponse = mapper.Map<UserResponseDTO>(user);
        return Ok(userResponse);
    }
    
    [AllowAnonymous]
    [HttpPost(Name = nameof(AuthenticateUser))]
    public async Task<IActionResult> AuthenticateUser([FromForm] string username, [FromForm] string password) {
        var user = userRepository.GetUserByUsername(username);
        if (user == null) return Unauthorized();

        if (HashPassword(password, user.Salt) != user.PasswordHash) return Unauthorized();
        
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(AppSettings.VALID_JWT_LENGTH_DAYS),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                                                        new SymmetricSecurityKey(key), 
                                                        SecurityAlgorithms.HmacSha512Signature),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return Ok(jwt);
    }
}