using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TescEvents.Utilities;

namespace TescEvents.Services; 

public class AuthService : IAuthService {
    private readonly JwtOptions jwtOptions;
    public AuthService(IOptions<JwtOptions> jwtOptions) {
        this.jwtOptions = jwtOptions.Value;
    }
    
    public string CreateJwt(Guid userId, string email) {
        var issuer = jwtOptions.Issuer;
        var audience = jwtOptions.Audience;
        var key = Encoding.ASCII.GetBytes(jwtOptions.Key);
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

    public Guid? ValidateJwt(string jwt) {
        if (jwt.IsNullOrEmpty()) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtOptions.Key);
        try {
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.Actor).Value);
            return userId;
        } catch {
            return null;
        }
    }
}