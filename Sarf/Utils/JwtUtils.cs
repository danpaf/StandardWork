using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Sarf.Database.Models;

namespace Sarf.Utils;

public class JwtUtils
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secret;
    private readonly DateTime _notBefore;
    private readonly int _lifetime;
    private readonly int _refreshLifeTime;
    
    public JwtUtils(IConfiguration configuration)
    {
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _secret = configuration["Jwt:Secret"];
        _lifetime = int.Parse(configuration["Jwt:Lifetime"]);
        if (!String.IsNullOrEmpty(configuration["Jwt:NotBefore"]))
            _notBefore = DateTime.Parse(configuration["Jwt:NotBefore"]);
        _refreshLifeTime = int.Parse(configuration["Jwt:RefreshLifeTime"]);
    }

    public string GenerateJwtToken(Guid userUid)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Audience = _audience,
            NotBefore = _notBefore,
            Subject = new ClaimsIdentity(new[] {new Claim("uid", userUid.ToString())}),
            Expires = DateTime.Now.AddSeconds(_lifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Guid? ValidateJwtToken(string token)
    {
        if (String.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var userUid = Guid.Parse(jwtToken.Claims.First(x => x.Type == "uid").Value);

            return userUid;
        }
        catch
        {
            return null;
        }
    }

    public RefreshToken GenerateRefreshToken(Guid userUid, string ipAddress)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(128);
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAt = DateTime.Now.AddSeconds(_refreshLifeTime),
            CreatedAt = DateTime.Now,
            CreatedIp = ipAddress,
            UserUid = userUid
        };

        return refreshToken;
    }
}