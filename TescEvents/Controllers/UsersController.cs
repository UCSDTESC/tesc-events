using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TescEvents.DTOs;
using TescEvents.Models;
using TescEvents.Services;
using static BCrypt.Net.BCrypt;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
[AllowAnonymous]
public class UsersController : ControllerBase {
    private readonly IMapper mapper;
    private readonly IAuthService authService;
    private readonly IUserService userService;
    private readonly IEmailService emailService;
    private readonly IValidator<UserCreateRequestDTO> userValidator;

    public UsersController(IMapper mapper, 
                           IAuthService authService, 
                           IUserService userService, 
                           IEmailService emailService, 
                           IValidator<UserCreateRequestDTO> userValidator) {
        this.mapper = mapper;
        this.authService = authService;
        this.userService = userService;
        this.emailService = emailService;
        this.userValidator = userValidator;
    }

    [Authorize]
    [HttpPatch("{userid:guid}")]
    public IActionResult UpdateUserInfo(Guid userId, [FromBody, Required] JsonPatchDocument<UserPatches> patchDoc) {
        var id = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        if (id == null || userId.ToString() != id) return Unauthorized();

        var student = userService.GetStudent(userId);
        if (student == null) return NotFound();

        var studentToUpdate = mapper.Map<UserPatches>(student);
        try {
            patchDoc.ApplyTo(studentToUpdate);
        } catch {
            return BadRequest();
        }
        if (studentToUpdate.First == null || studentToUpdate.Last == null) return BadRequest();
        
        mapper.Map(studentToUpdate, student);

        // TODO: avoid using try/catch
        try {
            userService.UpdateStudent(student);
        } catch {
            return BadRequest();
        }

        var userRes = mapper.Map<UserResponseDTO>(student);

        return Ok(userRes);
    }

    [Authorize]
    [HttpGet("{userId:guid}")]
    public IActionResult GetUserInfo(Guid userId) {
        var id = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        if (id == null || userId.ToString() != id) return Unauthorized();

        var student = userService.GetStudent(Guid.Parse(id));
        if (student == null) return NotFound();

        var userRes = mapper.Map<UserResponseDTO>(student);
        return Ok(userRes);
    }

    [HttpPost("register")]
    public IActionResult Register([FromForm] UserCreateRequestDTO userReq) {
        var validationResult = userValidator.Validate(userReq);
        if (!validationResult.IsValid) {
            return BadRequest(new {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "One or more validation errors occurred.",
                status = 400,
                errors = validationResult.Errors
                                         .GroupBy(e => e.PropertyName)
                                         .ToDictionary(
                                                       x => x.Key, 
                                                       x => x
                                                            .ToList()
                                                            .Select(m => m.ErrorMessage))
            });
        }

        var emailValidationFailure = new ValidationResult("Email address already in use");
        if (userService.GetStudentByEmail(userReq.Email) != null)
            return BadRequest(new {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "One or more validation errors occurred.",
                status = 400,
                errors = new Dictionary<string, string[]> {
                    { "Email", new []{ emailValidationFailure.ErrorMessage } }
                }
            }); 
        
        var user = mapper.Map<Student>(userReq);
        var salt = GenerateSalt();

        user.Salt = salt;
        user.PasswordHash = HashPassword(userReq.Password, salt);
        
        userService.CreateStudent(user);

        return NoContent();
    }

    [HttpPost("login")]
    public IActionResult Login([FromForm] string email, [FromForm] string password) {
        var user = userService.GetStudentByEmail(email);
        if (user == null) return Unauthorized();

        if (HashPassword(password, user.Salt) != user.PasswordHash) return Unauthorized();

        var jwt = authService.CreateJwt(user.Id, user.Email);
        return Ok(jwt);
    }

    [HttpPost("recovery")]
    public IActionResult RequestPasswordReset([FromBody] string email) {
        var user = userService.GetStudentByEmail(email);
        if (user == null) return Unauthorized();

        // Generate a random 5-letter code
        const string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var b = new StringBuilder();
        for (int i = 0; i < 5; i++) {
            var rand = new Random().Next(0, allowed.Length);
            b.Append(allowed[rand]);
        }
        var code = b.ToString();

        user.ResetToken = code;
        userService.UpdateStudent(user);

        emailService.SendPasswordResetEmail(email, code);

        return Accepted();
    }

    [HttpPost("password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordRequestDTO req) {
        var user = userService.GetStudentByEmail(req.Email);
        if (user == null) return Unauthorized();

        if (user.ResetToken != req.RecoveryToken) return Unauthorized();

        // TODO: Validate password
        if (req.NewPassword != req.ConfirmPassword) return BadRequest();

        var salt = GenerateSalt();
        user.Salt = salt;
        user.PasswordHash = HashPassword(req.NewPassword, salt);
        user.ResetToken = null;
        
        userService.UpdateStudent(user);

        return NoContent();
    }
}