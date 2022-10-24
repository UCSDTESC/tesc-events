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
    private readonly IStudentRepository studentRepository;
    private readonly IValidator<Student> studentValidator;
    private readonly IMapper mapper;
    
    public AuthController(IStudentRepository studentRepository, IMapper mapper, IValidator<Student> studentValidator) {
        this.studentRepository = studentRepository;
        this.mapper = mapper;
        this.studentValidator = studentValidator;
    }

    [AllowAnonymous]
    [HttpPost("register", Name = nameof(RegisterUser))]
    public async Task<IActionResult> RegisterUser([Required] [FromForm] UserCreateRequestDTO userReq) {
        var userEntity = mapper.Map<Student>(userReq);
        
        var validationResult = await studentValidator.ValidateAsync(userEntity);
        if (!validationResult.IsValid) return BadRequest(
                                                         validationResult.Errors
                                                                         .Select(error => error.ErrorMessage));

        var salt = GenerateSalt();
        userEntity.Salt = salt;
        userEntity.PasswordHash = HashPassword(userReq.Password, salt);
        studentRepository.CreateUser(userEntity);

        var userResponse = mapper.Map<UserResponseDTO>(userEntity);
        return CreatedAtRoute(new {
                                  action = "GetUser",
                                  controller = "Users",
                                  userResponse.Id
                              }, 
        userResponse);
    }
    
    [AllowAnonymous]
    [HttpPost(Name = nameof(AuthenticateUser))]
    public async Task<IActionResult> AuthenticateUser([FromForm] string username, [FromForm] string password) {
        var user = studentRepository.GetUserByUsername(username);
        if (user == null) return NotFound();

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