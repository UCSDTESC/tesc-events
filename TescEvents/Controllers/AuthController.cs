using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TescEvents.Utilities;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase {
    public AuthController() {
        
    }
    
    [AllowAnonymous]
    [HttpPost(Name = nameof(AuthenticateUser))]
    public async Task<IActionResult> AuthenticateUser([FromForm] string username, [FromForm] string password) {
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(AppSettings.VALID_JWT_LENGTH_DAYS),
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(
                                                        new SymmetricSecurityKey(key), 
                                                        SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return Ok(jwt);

        return Unauthorized();
    }
}