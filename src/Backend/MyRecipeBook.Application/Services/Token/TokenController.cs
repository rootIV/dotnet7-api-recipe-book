using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Application.Services.Token;

public class TokenController
{
    private const string EmailAlias = "eml";
    private readonly double TokenMinutesLifeTime;
    private readonly string SecurityKey;

    public TokenController(double tokenMinutesLifeTime, string securityKey)
    {
        TokenMinutesLifeTime = tokenMinutesLifeTime;
        SecurityKey = securityKey;
    }

    public string GenerateToken(string userEmail)
    {
        var claims = new List<Claim>
        {
            new Claim(EmailAlias, userEmail)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(TokenMinutesLifeTime),
            SigningCredentials = new SigningCredentials(
                SymmetricKey(),
                SecurityAlgorithms.HmacSha256Signature
                )
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SymmetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var claims = tokenHandler.ValidateToken(token, validationParameters, out _);

        return claims;
    }
    public string RecoverEmailOnToken(string token)
    {
        var claims = ValidateToken(token);

        return claims.FindFirst(EmailAlias).Value;
    }

    private SymmetricSecurityKey SymmetricKey()
    {
        var symmetricKey = Convert.FromBase64String( SecurityKey );

        return new SymmetricSecurityKey(symmetricKey);
    }
}
