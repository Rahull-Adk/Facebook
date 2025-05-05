using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Facebook.Data;
using Facebook.Models;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Facebook.Helpers;

public class GenerateToken
{
    private readonly IConfiguration _config;
    public GenerateToken(IConfiguration config)
    {
        _config = config;
        
    }

    public string GenerateJwtToken(User user)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecurityKey"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_config["Jwt:ExpireTime"])),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            throw new ApplicationException(e.Message);
        }
    }
    
}