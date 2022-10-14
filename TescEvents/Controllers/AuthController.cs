using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TescEvents.Models;
using TescEvents.Repositories;
using TescEvents.Utilities;
using static BCrypt.Net.BCrypt;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IUserRepository userRepository;
    public AuthController(IUserRepository userRepository) {
        this.userRepository = userRepository;
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