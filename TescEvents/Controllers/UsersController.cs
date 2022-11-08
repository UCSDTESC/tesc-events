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
    private readonly IUploadService uploadService;
    private readonly IEmailService emailService;

    public UsersController(IMapper mapper, IAuthService authService, IUserService userService, IUploadService 
                               uploadService, IEmailService emailService) {
        this.mapper = mapper;
        this.authService = authService;
        this.userService = userService;
        this.uploadService = uploadService;
        this.emailService = emailService;
    }

    [Authorize]
    [HttpPost("/{userId}")]
    public IActionResult GetUserInfo(string userId) {
        var id = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        if (id == null || userId != id) return Unauthorized();

        var student = userService.GetUser(Guid.Parse(id));
        if (student == null) return NotFound();

        var userRes = mapper.Map<UserResponseDTO>(student);
        return Ok(userRes);
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

        return NoContent();
    }

    [HttpPost("/login")]
    public IActionResult Login([FromForm] string email, [FromForm] string password) {
        var user = userService.GetUserByEmail(email);
        if (user == null) return Unauthorized();

        if (HashPassword(password, user.Salt) != user.PasswordHash) return Unauthorized();

        var jwt = authService.CreateJwt(user.Id, user.Email);
        return Ok(jwt);
    }

    [Authorize]
    [HttpPost("{userId:guid}/resume")]
    public IActionResult UploadResume(Guid userId, [FromForm] IFormFile resume) {
        var id = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        if (id == null) return Unauthorized();

        if (userId != Guid.Parse(id)) return Unauthorized();
        var user = userService.GetUser(userId);
        if (user == null) return Unauthorized();
        
        // TODO: Validate resume

        var oldResumeUrl = user.ResumeUrl;
        if (oldResumeUrl != null) {
            uploadService.DeleteFileAtPath(oldResumeUrl);
        }
        var path = Path.Join(AppSettings.ResumeBucket, userId.ToString());
        var resumeUrl = uploadService.UploadFileToPath(path, resume);

        user.ResumeUrl = resumeUrl;
        userService.UpdateUser(user);
        userService.Commit();
        
        return Ok();
    }

    [HttpPost("/recovery")]
    public IActionResult RequestPasswordReset([FromBody] string email) {
        var user = userService.GetUserByEmail(email);
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
        userService.UpdateUser(user);
        userService.Commit();

        emailService.SendPasswordResetEmail(email, code);

        return Accepted();
    }

    [HttpPost("/password")]
    public IActionResult ResetPassword([FromBody] string email, [FromBody] string recoveryToken, [FromBody] string newPassword,
                                       [FromBody] string confirmPassword) {
        var user = userService.GetUserByEmail(email);
        if (user == null) return Unauthorized();

        if (user.ResetToken != recoveryToken) return Unauthorized();

        // TODO: Validate password
        if (newPassword != confirmPassword) return BadRequest();

        var salt = GenerateSalt();
        user.Salt = salt;
        user.PasswordHash = HashPassword(newPassword, salt);
        user.ResetToken = null;
        
        userService.UpdateUser(user);
        userService.Commit();

        return NoContent();
    }
}