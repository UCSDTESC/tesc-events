using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TescEvents.Services; 

public class AuthService : IAuthService {
    public string CreateJwt(Guid userId, string email) {
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Actor, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
            }),
            Expires = DateTime.UtcNow.AddDays(14),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                                                        new SymmetricSecurityKey(key),
                                                        SecurityAlgorithms.HmacSha512Signature),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }
}